using System;
using System.Collections.Generic;
using SharpDX;

namespace LogicSimulator.Scene;

public abstract class ResourceDependentObject : IDisposable
{
    private readonly Dictionary<int, object> _resources = new();

    private readonly List<int> _resourcesToUpdate = new();

    ~ResourceDependentObject()
    {
        Dispose();
    }

    public T GetResourceValue<T>(Resource resource, ObjectRenderer objectRenderer) where T : class
    {
        if (typeof(T) != resource.ResourceType)
            throw new TypeAccessException("Type of resource and type of requested resource value are different!");

        if (GetType() != resource.OwnerType)
            throw new TypeAccessException("Owner type and resource owner type are different!");

        var hashCode = resource.GetHashCode();

        if (_resourcesToUpdate.Contains(hashCode))
        {
            var newResource = resource.UpdateResourceAction(objectRenderer, this);

            _resources.TryGetValue(hashCode, out var resourceValue);

            if (resourceValue is IDisposable o)
                Utilities.Dispose(ref o);

            _resources[hashCode] = newResource;

            _resourcesToUpdate.Remove(hashCode);

            return (T)newResource;
        }

        if (!_resources.TryGetValue(hashCode, out var value))
        {
            return null;
        }

        return (T)value;
    }

    public void Dispose()
    {
        foreach (var resource in _resources.Values)
        {
            if (resource is IDisposable o)
            {
                Utilities.Dispose(ref o);
            }
        }

        _resources.Clear();
        _resourcesToUpdate.Clear();
    }

    protected void OnResourceChanged(Resource resource)
    {
        if (_resourcesToUpdate.Contains(resource.GetHashCode()))
            return;

        _resourcesToUpdate.Add(resource.GetHashCode());
    }
}
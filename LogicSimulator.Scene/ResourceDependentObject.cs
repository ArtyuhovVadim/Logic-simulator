using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene;

public abstract class ResourceDependentObject : IDisposable
{
    private static readonly List<ResourceDependentObject> AllResourceDependentObject = new();

    private readonly Dictionary<int, object> _resources = new();

    private readonly List<int> _requireUpdateResources = new();

    protected ResourceDependentObject() => AllResourceDependentObject.Add(this);

    public static bool IsRequireRender { get; private set; }

    public static void RequireUpdateInAllResourceDependentObjects()
    {
        foreach (var resourceDependentObject in AllResourceDependentObject)
        {
            resourceDependentObject._requireUpdateResources.AddRange(resourceDependentObject._resources.Keys);
        }

        RequireRender();
    }

    public static void RequireRender() => IsRequireRender = true;

    public static void EndRender() => IsRequireRender = false;

    public T GetResourceValue<T>(Resource resource, RenderTarget renderTarget)
    {
        var hash = resource.GetHashCode();

        if (_requireUpdateResources.Contains(hash))
        {
            if (_resources.TryGetValue(hash, out var oldResource))
            {
                if (oldResource is IDisposable o)
                    Utilities.Dispose(ref o);
            }

            var newResource = resource.Update(renderTarget, this);

            _resources[hash] = newResource;

            _requireUpdateResources.Remove(hash);

            return (T)newResource;
        }

        if (_resources.TryGetValue(hash, out var resourceValue))
        {
            return (T)resourceValue;
        }

        resourceValue = resource.Update(renderTarget, this);

        _resources[hash] = resourceValue;

        return (T)resourceValue;
    }

    protected void RequireUpdate(Resource resource)
    {
        var hash = resource.GetHashCode();

        if (_requireUpdateResources.Contains(hash)) return;

        _requireUpdateResources.Add(hash);

        RequireRender();
    }

    protected T GetCashedResourceValue<T>(Resource resource)
    {
        if (_resources.TryGetValue(resource.GetHashCode(), out var resourceValue))
        {
            return (T)resourceValue;
        }

        throw new ApplicationException("Resource not found!");
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

        AllResourceDependentObject.Remove(this);
    }
}
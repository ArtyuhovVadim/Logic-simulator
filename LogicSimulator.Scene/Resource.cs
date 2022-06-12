using System;

namespace LogicSimulator.Scene;

public class Resource
{
    private readonly int _hashCode;

    private static int lastHashCode;

    public Type ResourceType { get; }
    public Type OwnerType { get; }
    public UpdateResourceActionDelegate UpdateResourceAction { get; private set; }

    public delegate object UpdateResourceActionDelegate(ObjectRenderer renderer, ResourceDependentObject o);

    private Resource(Type resourceType, Type ownerType, UpdateResourceActionDelegate updateResourceAction)
    {
        ResourceType = resourceType;
        OwnerType = ownerType;
        UpdateResourceAction = updateResourceAction;

        _hashCode = lastHashCode;
        lastHashCode++;
    }

    public static Resource Register<T>(Type ownerType, UpdateResourceActionDelegate updateResourceAction)
    {
        return new Resource(typeof(T), ownerType, updateResourceAction);
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }
}
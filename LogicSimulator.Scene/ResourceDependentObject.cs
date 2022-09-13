using System;

namespace LogicSimulator.Scene;

public abstract class ResourceDependentObject : IDisposable
{
    private static uint _lastId;

    public uint Id { get; }

    protected ResourceDependentObject() => Id = _lastId++;

    public void Dispose()
    {
        ResourceCache.ReleaseResources(this);
        GC.SuppressFinalize(this);
    }

    ~ResourceDependentObject()
    {
        ResourceCache.ReleaseResources(this);
    }
}
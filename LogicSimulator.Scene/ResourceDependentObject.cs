using System;
using SharpDX;

namespace LogicSimulator.Scene;

public abstract class ResourceDependentObject : IDisposable
{
    private static uint _lastId;

    public uint Id { get; }

    protected ResourceDependentObject() => Id = _lastId++;

    protected void SetAndUpdateResource(ref Vector2 field, Vector2 value, Resource resource)
    {
        if (field == value) return;
        field = value;
        ResourceCache.RequestUpdate(this, resource);
    }

    protected void SetAndUpdateResource(ref float field, float value, Resource resource)
    {
        if (MathUtil.NearEqual(field, value)) return;
        field = value;
        ResourceCache.RequestUpdate(this, resource);
    }

    protected void SetAndUpdateResource(ref Color4 field, Color4 value, Resource resource)
    {
        if (field == value) return;
        field = value;
        ResourceCache.RequestUpdate(this, resource);
    }

    protected void SetAndRequestRender(ref Vector2 field, Vector2 value)
    {
        if (field == value) return;
        field = value;
        RenderNotifier.RequestRender(this);
    }

    protected void SetAndRequestRender(ref float field, float value)
    {
        if (MathUtil.NearEqual(field, value)) return;
        field = value;
        RenderNotifier.RequestRender(this);
    }

    protected void SetAndRequestRender(ref bool field, bool value)
    {
        if (field == value) return;
        field = value;
        RenderNotifier.RequestRender(this);
    }

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
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SharpDX;

namespace LogicSimulator.Scene;

public abstract class ResourceDependentObject : IDisposable, INotifyPropertyChanged
{
    private static uint _lastId;

    public event PropertyChangedEventHandler PropertyChanged;

    public uint Id { get; }

    protected ResourceDependentObject() => Id = _lastId++;

    protected void SetAndUpdateResource(ref Vector2 field, Vector2 value, Resource resource, [CallerMemberName]string propertyName = null)
    {
        if (field == value) return;
        field = value;
        ResourceCache.RequestUpdate(this, resource);
        OnPropertyChanged(propertyName);
    }

    protected void SetAndUpdateResource(ref float field, float value, Resource resource, [CallerMemberName] string propertyName = null)
    {
        if (MathUtil.NearEqual(field, value)) return;
        field = value;
        ResourceCache.RequestUpdate(this, resource);
        OnPropertyChanged(propertyName);
    }

    protected void SetAndUpdateResource(ref Color4 field, Color4 value, Resource resource, [CallerMemberName] string propertyName = null)
    {
        if (field == value) return;
        field = value;
        ResourceCache.RequestUpdate(this, resource);
        OnPropertyChanged(propertyName);
    }

    protected void SetAndRequestRender(ref Vector2 field, Vector2 value, [CallerMemberName] string propertyName = null)
    {
        if (field == value) return;
        field = value;
        RenderNotifier.RequestRender(this);
        OnPropertyChanged(propertyName);
    }

    protected void SetAndRequestRender(ref float field, float value, [CallerMemberName] string propertyName = null)
    {
        if (MathUtil.NearEqual(field, value)) return;
        field = value;
        RenderNotifier.RequestRender(this);
        OnPropertyChanged(propertyName);
    }

    protected void SetAndRequestRender(ref bool field, bool value, [CallerMemberName] string propertyName = null)
    {
        if (field == value) return;
        field = value;
        RenderNotifier.RequestRender(this);
        OnPropertyChanged(propertyName);
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
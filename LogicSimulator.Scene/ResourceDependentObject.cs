using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization;

namespace LogicSimulator.Scene;

public abstract class ResourceDependentObject : IDisposable, INotifyPropertyChanged
{
    private static uint _lastId;

    public event PropertyChangedEventHandler PropertyChanged;

    [YamlIgnore]
    public uint Id { get; }

    protected ResourceDependentObject() => Id = _lastId++;

    protected void SetAndUpdateResource<T>(ref T field, T value, Resource resource, [CallerMemberName] string propertyName = null)
    {
        if (Equals(field, value)) return;
        field = value;
        ResourceCache.RequestUpdate(this, resource);
        OnPropertyChanged(propertyName);
    }

    protected void SetAndRequestRender<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (Equals(field, value)) return;
        field = value;
        RenderNotifier.RequestRender(this);
        OnPropertyChanged(propertyName);
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization;

namespace LogicSimulator.Scene;

public abstract class ResourceDependentObject : IDisposable, INotifyPropertyChanged
{
    private static uint _lastId = 1;

    private Scene2D _scene;

    private readonly List<ulong> _resourceIds = new();

    public event PropertyChangedEventHandler PropertyChanged;

    [YamlIgnore]
    public uint Id { get; }

    [YamlIgnore]
    public bool IsInitialized { get; private set; }

    [YamlIgnore]
    public ulong[] ResourceIds => _resourceIds.ToArray();

    protected ResourceDependentObject() => Id = _lastId++;

    protected abstract void OnInitialize(Scene2D scene);

    protected void Initialize(Scene2D scene)
    {
        if (IsInitialized) return;

        _scene = scene;

        OnInitialize(scene);

        IsInitialized = true;
    }

    protected void InitializeResource(Resource resource)
    {
        ResourceCache.InitializeResource(this, resource, _scene);
        _resourceIds.Add((ulong)Id << 32 | resource.Id);
    }

    protected T GetOrUpdateResource<T>(Resource resource)
    {
        return ResourceCache.GetOrUpdate<T>(this, resource, _scene);
    }

    protected bool SetAndImmediatelyUpdateResource<T>(ref T field, T value, Resource resource, [CallerMemberName] string propertyName = null)
    {
        if (Equals(field, value)) return false;
        field = value;
        if (!IsInitialized) return false;
        ResourceCache.ImmediatelyUpdate(this, resource, _scene);
        RenderNotifier.RequestRender(_scene);
        OnPropertyChanged(propertyName);
        return true;
    }

    protected bool SetAndUpdateResource<T>(ref T field, T value, Resource resource, [CallerMemberName] string propertyName = null)
    {
        if (Equals(field, value)) return false;
        field = value;
        if (!IsInitialized) return false;
        ResourceCache.RequestUpdate(this, resource);
        RenderNotifier.RequestRender(_scene);
        OnPropertyChanged(propertyName);
        return true;
    }

    protected bool SetAndRequestRender<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (Equals(field, value)) return false;
        field = value;
        if (!IsInitialized) return false;
        RenderNotifier.RequestRender(_scene);
        OnPropertyChanged(propertyName);
        return true;
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
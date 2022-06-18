using System;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene;

public class Resource
{
    private readonly int _hash;
    private readonly ResourceChangedCallback _callback;

    public delegate object ResourceChangedCallback(RenderTarget renderTarget, ResourceDependentObject o);

    private Resource(int hash, ResourceChangedCallback callback)
    {
        _callback = callback;
        _hash = hash;
    }

    public static Resource Register<TOwner, TResource>(string name, ResourceChangedCallback callback) where TOwner : ResourceDependentObject where TResource : IDisposable
    {
        var hash = typeof(TOwner).GetHashCode() ^ typeof(TResource).GetHashCode() ^ name.GetHashCode();

        return new Resource(hash, callback);
    }

    public object Update(RenderTarget renderTarget, ResourceDependentObject o) => _callback.Invoke(renderTarget, o);

    public override int GetHashCode() => _hash;
}
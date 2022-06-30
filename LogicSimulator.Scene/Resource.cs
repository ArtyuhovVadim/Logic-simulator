using System;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene;

public class Resource
{
    private readonly int _hash;
    private readonly ResourceChangedCallback _changedCallback;

    public delegate object ResourceChangedCallback(RenderTarget renderTarget, ResourceDependentObject o);

    private Resource(int hash, ResourceChangedCallback changedCallback)
    {
        _changedCallback = changedCallback;
        _hash = hash;
    }

    public static Resource Register<TOwner, TResource>(string name, ResourceChangedCallback changedCallback) where TOwner : ResourceDependentObject
                                                                                                             where TResource : IDisposable
    {
        var hash = typeof(TOwner).GetHashCode() ^ typeof(TResource).GetHashCode() ^ name.GetHashCode();

        return new Resource(hash, changedCallback);
    }

    public object Update(RenderTarget renderTarget, ResourceDependentObject o) => _changedCallback.Invoke(renderTarget, o);

    public override int GetHashCode() => _hash;
}
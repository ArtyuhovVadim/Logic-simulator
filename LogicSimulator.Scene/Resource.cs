using SharpDX.Direct2D1;

namespace LogicSimulator.Scene;

public class Resource
{
    private static uint _lastId = 1;

    public uint Id { get; }

    private readonly ResourceChangedCallback _changedCallback;

    public Resource(ResourceChangedCallback changedCallback)
    {
        _changedCallback = changedCallback;
        Id = _lastId++;
    }

    public object Update(RenderTarget renderTarget, ResourceDependentObject o)
    {
        return _changedCallback.Invoke(renderTarget, o);
    }
}
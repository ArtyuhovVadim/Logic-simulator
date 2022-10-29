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

    public object Update(Scene2D scene, ResourceDependentObject o)
    {
        return _changedCallback.Invoke(scene, o);
    }
}
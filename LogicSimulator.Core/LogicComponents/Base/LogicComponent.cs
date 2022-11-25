namespace LogicSimulator.Core.LogicComponents.Base;

public abstract class LogicComponent
{
    private static uint _lastId;

    public event Action<LogicComponent> Updated;

    public uint Id { get; }

    protected LogicComponent() => Id = _lastId++;

    protected abstract void OnUpdate();

    public void Update()
    {
        Console.WriteLine($"{GetType().Name} Updated Id: {Id}");
        OnUpdate();
        Updated?.Invoke(this);
    }
}
namespace LogicSimulator.Core.LogicComponents.Base;

public abstract class LogicComponent
{
    private static uint _lastId;

    protected bool isFirstUpdated { get; private set; } = false;

    public event Action<LogicComponent> Updated;

    public bool IsDirty { get; set; } = true;

    public uint Id { get; }

    protected LogicComponent() => Id = _lastId++;

    protected abstract void OnUpdate();

    public void Update()
    {
        OnUpdate();
        Updated?.Invoke(this);
        IsDirty = false;
        isFirstUpdated = true;
    }
}
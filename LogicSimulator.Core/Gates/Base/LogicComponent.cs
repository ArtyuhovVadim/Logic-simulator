namespace LogicSimulator.Core.Gates.Base;

public abstract class LogicComponent
{
    public event Action? Invalidated;

    public void Invalidate(Simulator simulator)
    {
        OnInvalidate(simulator);
        Invalidated?.Invoke();
    }

    protected abstract void OnInvalidate(Simulator simulator);
}
namespace LogicSimulator.Core.Gates.Base;

public abstract class BaseGate
{
    public void Invalidate() => OnInvalidate();

    protected abstract void OnInvalidate();
}
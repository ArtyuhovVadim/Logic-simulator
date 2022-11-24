namespace LogicSimulator.Core.LogicComponents.Base;

public abstract class LogicComponent
{
    public abstract void Update();

    public abstract void SetInputPortState(int i, PortState state);
}
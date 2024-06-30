namespace LogicSimulator.Core.Gates.Base;

public abstract class BaseGate : LogicComponent
{
    public abstract IEnumerable<BasePort> Ports { get; }

    public ulong Delay { get; set; }
}
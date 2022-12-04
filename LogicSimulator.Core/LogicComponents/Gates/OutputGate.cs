using LogicSimulator.Core.LogicComponents.Gates.Base;

namespace LogicSimulator.Core.LogicComponents.Gates;

public class OutputGate : BaseGate
{
    public LogicState State { get; set; }

    public OutputGate() : base(0, 1) { }

    protected override void OnUpdate()
    {
        Ports[0].State = State;
        Ports[0].Update();
    }
}
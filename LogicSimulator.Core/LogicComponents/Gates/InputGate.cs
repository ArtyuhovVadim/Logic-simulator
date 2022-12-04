using LogicSimulator.Core.LogicComponents.Gates.Base;

namespace LogicSimulator.Core.LogicComponents.Gates;

public class InputGate : BaseGate
{
    public LogicState State { get; set; }

    public InputGate() : base(1, 0) { }

    protected override void OnUpdate()
    {
        State = Ports[0].State;
    }
}
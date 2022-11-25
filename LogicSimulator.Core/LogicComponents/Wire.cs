using LogicSimulator.Core.LogicComponents.Base;

namespace LogicSimulator.Core.LogicComponents;

public class Wire : LogicComponent
{
    public Port InputPort { get; private set; }

    public Port OutputPort { get; private set; }

    public LogicState State { get; private set; }

    public Wire(Port inputPort, Port outputPort)
    {
        InputPort = inputPort;
        OutputPort = outputPort;
    }

    protected override void OnUpdate()
    {
        State = InputPort.State;
        OutputPort.State = State;
        OutputPort.Update();
    }
}
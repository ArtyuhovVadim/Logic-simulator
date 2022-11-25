using LogicSimulator.Core.LogicComponents.Base;

namespace LogicSimulator.Core.LogicComponents;

public class Wire : LogicComponent
{
    private bool _isFirstUpdated = false;

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
        if (_isFirstUpdated && State == InputPort.State && State == OutputPort.State)
            return;

        _isFirstUpdated = true;

        State = InputPort.State;
        OutputPort.State = State;
        OutputPort.Update();
    }
}
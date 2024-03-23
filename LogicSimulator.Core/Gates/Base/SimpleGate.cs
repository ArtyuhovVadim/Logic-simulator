namespace LogicSimulator.Core.Gates.Base;

public abstract class SimpleGate : BaseGate
{
    private List<InputPort> _inputPorts = [];
    private int _inputPortsCount = 2;

    protected SimpleGate()
    {
        Output = new OutputPort(this);
        CreateInputPorts();
    }

    public int InputPortsCount
    {
        get => _inputPortsCount;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 2);
            _inputPortsCount = value;
            foreach (var inputPort in _inputPorts)
                inputPort.RemoveAllConnections();
            CreateInputPorts();
        }
    }

    public override IEnumerable<BasePort> Ports => [.. Inputs, Output];

    public IReadOnlyList<InputPort> Inputs => _inputPorts;

    public OutputPort Output { get; }

    public ulong Delay { get; set; }

    protected sealed override void OnInvalidate(Simulator simulator)
    {
        var newState = Inputs[0].State;

        for (var i = 1; i < Inputs.Count; i++)
            newState = GetOutputFromInput(newState, Inputs[i].State);

        if (Output.State == newState)
            return;

        simulator.PushEvent(Output, newState, Delay);
    }

    protected abstract SignalType GetOutputFromInput(SignalType a, SignalType b);

    private void CreateInputPorts()
    {
        var inputPorts = new List<InputPort>(_inputPortsCount);

        for (var i = 0; i < _inputPortsCount; i++)
            inputPorts.Add(new InputPort(this));

        _inputPorts = inputPorts;
    }
}
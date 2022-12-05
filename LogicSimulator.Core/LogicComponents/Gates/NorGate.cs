using LogicSimulator.Core.LogicComponents.Gates.Base;

namespace LogicSimulator.Core.LogicComponents.Gates;

public class NorGate : BaseGate
{
    public NorGate(int inputCount, int outputCount) : base(inputCount, outputCount)
    {
        if (outputCount != 1)
            throw new ArgumentException(nameof(outputCount));

        if (inputCount < 2)
            throw new ArgumentException(nameof(inputCount));
    }

    protected override void OnUpdate()
    {
        if (!IsDirty) return;

        var inputPort1 = Ports[0];
        var inputPort2 = Ports[1];
        var outputPort = Ports[2];

        LogicState newState;

        if (inputPort1.State == LogicState.Undefined && inputPort2.State == LogicState.Undefined)
        {
            newState = LogicState.Undefined;
        }
        else if (inputPort1.State == LogicState.False && inputPort2.State == LogicState.False)
        {
            newState = LogicState.True;
        }
        else if (inputPort1.State == LogicState.True || inputPort2.State == LogicState.True)
        {
            newState = LogicState.False;
        }
        else
        {
            newState = LogicState.Undefined;
        }

        if (newState != outputPort.State || !IsFirstUpdated)
        {
            outputPort.State = newState;
            outputPort.Update();
        }
    }
}
using LogicSimulator.Core.LogicComponents.Gates.Base;

namespace LogicSimulator.Core.LogicComponents.Gates;

public class NorGate : BaseGate
{
    private bool _isFirstUpdated;

    public NorGate(int inputCount, int outputCount) : base(inputCount, outputCount)
    {
        if (outputCount != 1)
            throw new ArgumentException(nameof(outputCount));

        if (inputCount < 2)
            throw new ArgumentException(nameof(inputCount));
    }

    protected override void OnUpdate()
    {
        Port outputPort = null;
        var outputState = LogicState.Undefined;

        var hasUndefined = false;

        for (var i = 0; i < Ports.Count; i++)
        {
            if (Ports[i].Type == PortType.Output)
            {
                outputPort = Ports[i];
                continue;
            }

            if (Ports[i].State == LogicState.True)
            {
                outputState = LogicState.False;
                hasUndefined = false;
                outputPort = Ports.First(x => x.Type == PortType.Output);
                break;
            }

            if (Ports[i].State == LogicState.Undefined)
            {
                hasUndefined = true;
            }
        }

        if (!hasUndefined && outputState == LogicState.Undefined)
        {
            outputState = LogicState.True;
        }

        if (_isFirstUpdated && outputPort!.State == outputState)
            return;

        _isFirstUpdated = true;

        outputPort!.State = outputState;
        outputPort.Update();
    }
}
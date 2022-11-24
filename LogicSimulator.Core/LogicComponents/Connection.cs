using System.Diagnostics;
using LogicSimulator.Core.LogicComponents.Base;

namespace LogicSimulator.Core.LogicComponents;

[DebuggerDisplay("Input: {InputPort} Output: {OutputPort}")]
public class Connection : LogicComponent
{
    public Port InputPort { get; set; }
    public Port OutputPort { get; set; }

    public Connection(Port input, Port output)
    {
        InputPort = input;
        OutputPort = output;
    }

    public override void Update()
    {
        OutputPort.State = InputPort.State;
    }

    public override void SetInputPortState(int i, PortState state)
    {
        if (i != 0)
            throw new IndexOutOfRangeException(nameof(i));

        InputPort.State = state;
    }
}
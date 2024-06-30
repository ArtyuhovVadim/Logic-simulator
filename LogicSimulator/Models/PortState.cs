using LogicSimulator.Core;

namespace LogicSimulator.Models;

public class PortState
{
    public PortState(ulong time, SignalType state)
    {
        Time = time;
        State = state;
    }

    public ulong Time { get; set; }

    public SignalType State { get; set; }
}
using LogicSimulator.Core;

namespace LogicSimulator.Shared.Models;

public interface IWaveState
{
    ulong Time { get; }

    SignalType State { get; }
}
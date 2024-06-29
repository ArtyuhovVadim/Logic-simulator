using LogicSimulator.Core;

namespace LogicSimulator.Shared;

public interface IWaveState
{
    ulong Time { get; }

    SignalType State { get; }
}

public interface IWave
{
    double Scale { get; }

    double Offset { get; }

    IEnumerable<IWaveState> States { get; }
}
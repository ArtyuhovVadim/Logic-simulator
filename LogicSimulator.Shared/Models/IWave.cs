namespace LogicSimulator.Shared.Models;

public interface IWave
{
    double Scale { get; }

    double Offset { get; }

    IEnumerable<IWaveState> States { get; }
}
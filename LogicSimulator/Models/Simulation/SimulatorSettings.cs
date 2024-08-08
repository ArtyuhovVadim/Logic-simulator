namespace LogicSimulator.Models.Simulation;

public struct SimulatorSettings()
{
    public ulong MaxTime = ulong.MaxValue;

    public bool IsPauseSupported = true;

    public bool IsPausedOnStart = false;

    public ulong AdditionalSimulationTime = 0;
}
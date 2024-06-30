using LogicSimulator.Models;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface ISchemeSimulatorService
{
    IReadOnlyDictionary<string, PortSimulationResult> Result { get; }

    SimulatorSettings Settings { get; }

    SimulationState State { get; }

    bool CanStart { get; }

    bool CanResume { get; }

    bool CanPause { get; }

    bool CanStop { get; }

    bool CanSimulateNextStep { get; }

    void StartSimulation(LogicScheme scheme, SimulatorSettings settings);

    void SimulateNextStep();

    void ResumeSimulation();

    void PauseSimulation();

    void StopSimulation();
}
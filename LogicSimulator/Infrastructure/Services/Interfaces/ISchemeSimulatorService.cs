using LogicSimulator.Models.Logic;
using LogicSimulator.Models.Simulation;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public delegate void SimulationStateChanged(SimulationState oldState, SimulationState newState);

public interface ISchemeSimulatorService
{
    event SimulationStateChanged? SimulationStateChanged;

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

    Dictionary<string, PortSimulationResult> GetSimulationResult();
}
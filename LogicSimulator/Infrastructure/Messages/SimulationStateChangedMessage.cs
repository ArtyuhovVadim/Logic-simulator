using LogicSimulator.Models.Simulation;
using LogicSimulator.ViewModels.Anchorable;

namespace LogicSimulator.Infrastructure.Messages;

public record SimulationStateChangedMessage(SchemeViewModel Scheme, SimulationState OldState, SimulationState NewState, Dictionary<string, PortSimulationResult> SimulationResult);
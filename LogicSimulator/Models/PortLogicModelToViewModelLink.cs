using LogicSimulator.Core;
using LogicSimulator.ViewModels.ObjectViewModels.Gates;

namespace LogicSimulator.Models;

public record PortLogicModelToViewModelLink(BasePort LogicModel, PortViewModel ViewModel);
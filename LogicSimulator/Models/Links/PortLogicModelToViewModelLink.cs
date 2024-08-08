using LogicSimulator.Core;
using LogicSimulator.ViewModels.Logic;

namespace LogicSimulator.Models.Links;

public record PortLogicModelToViewModelLink(BasePort LogicModel, PortViewModel ViewModel);
using LogicSimulator.Core;
using LogicSimulator.Core.Gates.Base;
using LogicSimulator.ViewModels.Logic.Gates.Base;

namespace LogicSimulator.Models.Links;

public record GateLogicModelToViewModelLink(BaseGate LogicModel, BaseGateViewModel ViewModel, Dictionary<BasePort, PortLogicModelToViewModelLink> PortsMap);
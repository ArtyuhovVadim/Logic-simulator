using LogicSimulator.Core;
using LogicSimulator.Core.Gates.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;

namespace LogicSimulator.Models;

public record GateLogicModelToViewModelLink(BaseGate LogicModel, BaseGateViewModel ViewModel, Dictionary<BasePort, PortLogicModelToViewModelLink> PortsMap);
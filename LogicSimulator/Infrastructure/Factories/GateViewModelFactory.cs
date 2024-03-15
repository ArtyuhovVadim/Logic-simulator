using LogicSimulator.Core;
using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.ViewModels.ObjectViewModels.Gates;

namespace LogicSimulator.Infrastructure.Factories;

public class GateViewModelFactory : IGateViewModelFactory
{
    private readonly GateFactory _gateModelsFactory;

    public GateViewModelFactory(Simulator simulator) => _gateModelsFactory = new GateFactory(simulator);

    public AndGateViewModel CreateAndGateViewModel(int inputPortCount = 2) => 
        new(_gateModelsFactory.CreateAndGate(inputPortCount));
}
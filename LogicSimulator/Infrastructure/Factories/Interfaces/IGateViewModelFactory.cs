using LogicSimulator.ViewModels.ObjectViewModels.Gates;

namespace LogicSimulator.Infrastructure.Factories.Interfaces;

public interface IGateViewModelFactory
{
    AndGateViewModel CreateAndGateViewModel(int inputPortCount = 2);
}
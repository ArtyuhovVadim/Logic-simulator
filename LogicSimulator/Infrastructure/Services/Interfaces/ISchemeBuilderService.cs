using LogicSimulator.Models;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Gates;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface ISchemeBuilderService
{
    LogicScheme BuildFromViewModels(IEnumerable<BaseObjectViewModel> objects);

    void CreateLogicModelFrom(InputGateViewModel gate);

    void CreateLogicModelFrom(OutputGateViewModel gate);

    void CreateLogicModelFrom(AndGateViewModel gate);
}
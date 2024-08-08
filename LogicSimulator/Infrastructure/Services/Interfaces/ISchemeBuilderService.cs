using LogicSimulator.Models.Logic;
using LogicSimulator.ViewModels.Logic.Gates;
using LogicSimulator.ViewModels.Objects.Base;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface ISchemeBuilderService
{
    LogicScheme BuildFromViewModels(IEnumerable<BaseObjectViewModel> objects);

    void CreateLogicModelFrom(InputGateViewModel gate);

    void CreateLogicModelFrom(OutputGateViewModel gate);

    void CreateLogicModelFrom(AndGateViewModel gate);
}
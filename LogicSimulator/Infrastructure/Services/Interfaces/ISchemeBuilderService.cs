using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.Logic;
using LogicSimulator.ViewModels.Anchorable;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface ISchemeBuilderService
{
    void AddValidationRule(ISchemeValidationRule rule);

    LogicScheme BuildFromSchemeViewModel(SchemeViewModel scheme);
}
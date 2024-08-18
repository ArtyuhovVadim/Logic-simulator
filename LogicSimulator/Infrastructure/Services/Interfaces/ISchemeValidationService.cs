using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.Logic;
using LogicSimulator.ViewModels.Anchorable;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface ISchemeValidationService
{
    void AddValidationRule(ISchemeValidationRule rule);

    IReadOnlyList<SchemeValidationResult> Validate(SchemeViewModel scheme, IPreprocessedLogicScheme preprocessedScheme);
}
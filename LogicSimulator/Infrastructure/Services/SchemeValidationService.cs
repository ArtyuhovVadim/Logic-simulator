using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Logic;
using LogicSimulator.ViewModels.Anchorable;

namespace LogicSimulator.Infrastructure.Services;

public class SchemeValidationService : ISchemeValidationService
{
    private readonly HashSet<ISchemeValidationRule> _rules = [];

    public void AddValidationRule(ISchemeValidationRule rule)
    {
        if (!_rules.Add(rule))
            throw new InvalidOperationException($"{rule.GetType()} rule has been already added.");
    }

    public IReadOnlyList<SchemeValidationResult> Validate(SchemeViewModel scheme, IPreprocessedLogicScheme preprocessedScheme)
    {
        var context = new ValidationContext(scheme, preprocessedScheme);
        return _rules.Select(x => x.Validate(context)).ToList();
    }
}
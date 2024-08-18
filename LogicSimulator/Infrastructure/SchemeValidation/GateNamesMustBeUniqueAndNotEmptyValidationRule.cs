using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Models.MessageSources;
using LogicSimulator.ViewModels.Common;

namespace LogicSimulator.Infrastructure.SchemeValidation;

public class GateNamesMustBeUniqueAndNotEmptyValidationRule : ISchemeValidationRule
{
    public ValidationRuleLevel Level => ValidationRuleLevel.Error;

    public SchemeValidationResult Validate(ValidationContext context)
    {
        var invalidGatesGroups =
            context.PreprocessedScheme.Gates.GroupBy(x => x.Name).Where(x => x.Count() != 1 || string.IsNullOrWhiteSpace(x.Key)).ToList();

        return new Result(context, invalidGatesGroups, invalidGatesGroups.Count == 0, Level);
    }

    public class Result : SchemeValidationResult
    {
        public Result(ValidationContext context, List<IGrouping<string, BaseGateModel>> invalidGatesGroups, bool isValid, ValidationRuleLevel level) : base(isValid, level)
        {
            InvalidGatesGroups = invalidGatesGroups;
            Messages = invalidGatesGroups
                .SelectMany(x => x)
                .Select(gate => new OutputMessageViewModel(string.IsNullOrWhiteSpace(gate.Name) ? "Имя не может быть пустым." : $"Имя {gate.Name} не уникально.", Level, new GateMessageSource(context.Scheme, gate)));
        }

        public IReadOnlyList<IGrouping<string, BaseGateModel>> InvalidGatesGroups { get; }
    }
}
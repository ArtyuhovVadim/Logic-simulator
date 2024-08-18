using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.MessageSources;
using LogicSimulator.Models.SchemeGraph.Base;
using LogicSimulator.ViewModels.Common;

namespace LogicSimulator.Infrastructure.SchemeValidation;

public class MoreThenOneOutputPortConnectedValidationRule : ISchemeValidationRule
{
    public ValidationRuleLevel Level => ValidationRuleLevel.Error;

    public SchemeValidationResult Validate(ValidationContext context)
    {
        var invalidWiresGroups = context.PreprocessedScheme.Edges
            .GroupBy(x => x.WiresGroup)
            .Where(group => group.GroupBy(x => x.Source).Count() > 1)
            .Select(group => group.Key)
            .Concat(context.PreprocessedScheme.InvalidConnection.Where(x => x.OutputPorts.Count > 1).Select(x => x.WiresGroup))
            .ToList();

        return new Result(context, invalidWiresGroups, invalidWiresGroups.Count == 0, Level);
    }

    public class Result : SchemeValidationResult
    {
        public IReadOnlyList<IWiresGroup> InvalidWiresGroups { get; }

        public Result(ValidationContext context, List<IWiresGroup> invalidWiresGroups, bool isValid, ValidationRuleLevel level) : base(isValid, level)
        {
            InvalidWiresGroups = invalidWiresGroups;
            Messages = InvalidWiresGroups.Select(x => new OutputMessageViewModel("Более одного выходного порта объединены в цепь.", Level, new WiresMessageSource(context.Scheme, x.Wires)));
        }
    }
}
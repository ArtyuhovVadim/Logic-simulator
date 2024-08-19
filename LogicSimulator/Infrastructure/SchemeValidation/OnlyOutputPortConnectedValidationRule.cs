using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.MessageSources;
using LogicSimulator.Models.SchemeGraph.Base;
using LogicSimulator.ViewModels.Common;

namespace LogicSimulator.Infrastructure.SchemeValidation;

public class OnlyOutputPortConnectedValidationRule : ISchemeValidationRule
{
    public ValidationRuleLevel Level => ValidationRuleLevel.Warning;

    public SchemeValidationResult Validate(ValidationContext context)
    {
        var invalidWiresGroups = context.PreprocessedScheme.InvalidConnection.Where(x => x.OutputPorts.Count == 1 && x.InputPorts.Count == 0).Select(x => x.WiresGroup).ToList();
        return new Result(context, invalidWiresGroups, invalidWiresGroups.Count == 0, Level);
    }

    public class Result : SchemeValidationResult
    {
        public Result(ValidationContext context, List<IWiresGroup> invalidWiresGroups, bool isValid, ValidationRuleLevel level) : base(isValid, level)
        {
            InvalidWiresGroups = invalidWiresGroups;
            Messages = InvalidWiresGroups.Select(x => new OutputMessageViewModel("Подключен только выходной порт.", Level, new WiresMessageSource(context.Scheme, x.Wires)));
        }

        public IReadOnlyList<IWiresGroup> InvalidWiresGroups { get; }
    }
}

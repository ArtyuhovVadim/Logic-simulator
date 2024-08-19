using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.Logic;
using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Models.MessageSources;
using LogicSimulator.Shared.ExtensionMethods;
using LogicSimulator.ViewModels.Common;

namespace LogicSimulator.Infrastructure.SchemeValidation;

public class PortNamesMustBeUniqueAndNotEmptyValidationRule : ISchemeValidationRule
{
    public ValidationRuleLevel Level => ValidationRuleLevel.Error;

    public SchemeValidationResult Validate(ValidationContext context)
    {
        var gatesWithNotUniquePortNames = new List<(BaseGateModel Gate, List<IGrouping<string, PortModel>> PortGroups)>();
        var gatesWithEmptyPortNames = new List<(BaseGateModel Gate, List<IGrouping<string, PortModel>> PortGroups)>();

        foreach (var gate in context.PreprocessedScheme.Gates)
        {
            var groups = gate.Ports.GroupBy(x => x.Name).ToArray();
            var notUniqueNamesGroups = groups.Where(x => x.Count() > 1 && !string.IsNullOrWhiteSpace(x.Key)).ToList();
            var emptyNamesGroups = groups.Where(x => string.IsNullOrWhiteSpace(x.Key)).ToList();

            if (notUniqueNamesGroups.Count != 0)
                gatesWithNotUniquePortNames.Add((gate, notUniqueNamesGroups));

            if (emptyNamesGroups.Count != 0)
                gatesWithEmptyPortNames.Add((gate, emptyNamesGroups));
        }

        return new Result(context, gatesWithNotUniquePortNames, gatesWithEmptyPortNames, gatesWithNotUniquePortNames.Count == 0 && gatesWithEmptyPortNames.Count == 0, Level);
    }

    public class Result : SchemeValidationResult
    {
        public Result(
            ValidationContext context,
            List<(BaseGateModel Gate, List<IGrouping<string, PortModel>> PortGroups)> gatesWithNotUniquePortNames,
            List<(BaseGateModel Gate, List<IGrouping<string, PortModel>> PortGroups)> gatesWithEmptyPortNames,
            bool isValid,
            ValidationRuleLevel level) : base(isValid, level)
        {
            GatesWithNotUniquePortNames = gatesWithNotUniquePortNames;
            GatesWithEmptyPortNames = gatesWithEmptyPortNames;

            Messages = GatesWithNotUniquePortNames.Select(tuple => new OutputMessageViewModel($"Имена портов {tuple.PortGroups.ToStr(x => x.Key)} у вентиля{tuple.Gate.Name.ReturnIfNotEmpty()}не уникальны", Level, new GateMessageSource(context.Scheme, tuple.Gate)))
                                                  .Concat(GatesWithEmptyPortNames.Select(tuple => new OutputMessageViewModel($"Не все порты у вентиля{tuple.Gate.Name.ReturnIfNotEmpty()}имеют не пустое имя.", Level, new GateMessageSource(context.Scheme, tuple.Gate))));
        }

        public IReadOnlyList<(BaseGateModel Gate, List<IGrouping<string, PortModel>> PortGroups)> GatesWithNotUniquePortNames { get; }

        public IReadOnlyList<(BaseGateModel Gate, List<IGrouping<string, PortModel>> PortGroups)> GatesWithEmptyPortNames { get; }
    }
}

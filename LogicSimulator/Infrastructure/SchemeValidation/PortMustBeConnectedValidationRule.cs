using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.Logic;
using LogicSimulator.Models.MessageSources;
using LogicSimulator.Shared.ExtensionMethods;
using LogicSimulator.ViewModels.Common;

namespace LogicSimulator.Infrastructure.SchemeValidation;

public class PortMustBeConnectedValidationRule : ISchemeValidationRule
{
    public ValidationRuleLevel Level => ValidationRuleLevel.Warning;

    public SchemeValidationResult Validate(ValidationContext context)
    {
        var notConnectedPorts = context.PreprocessedScheme.Nodes.SelectMany(x => x.PortNodes).Where(x => !x.Connections.Any()).ToHashSet();
        var invalidConnectedPorts = context.PreprocessedScheme.InvalidConnection.SelectMany(x => x.InputPorts.Concat(x.OutputPorts));
        notConnectedPorts.ExceptWith(invalidConnectedPorts);
        return new Result(context, notConnectedPorts.Select(x => x.PortModel).ToList(), notConnectedPorts.Count == 0, Level);
    }

    public class Result : SchemeValidationResult
    {
        public Result(ValidationContext context, IReadOnlyList<PortModel> portsWithoutConnections, bool isValid, ValidationRuleLevel level) : base(isValid, level)
        {
            PortsWithoutConnections = portsWithoutConnections;
            Messages = PortsWithoutConnections.Select(x => new OutputMessageViewModel($"Порт{x.Name.ReturnIfNotEmpty()}вентиля{x.Parent.Name.ReturnIfNotEmpty()}не подключен.", Level, new GateMessageSource(context.Scheme, x.Parent)));
        }

        public IReadOnlyList<PortModel> PortsWithoutConnections { get; }
    }
}
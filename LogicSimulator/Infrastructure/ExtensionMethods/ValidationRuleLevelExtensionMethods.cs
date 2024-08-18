using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.Common;

namespace LogicSimulator.Infrastructure.ExtensionMethods;

public static class ValidationRuleLevelExtensionMethods
{
    public static MessageType ToMessageType(this ValidationRuleLevel level) => level switch
    {
        ValidationRuleLevel.Warning => MessageType.Warning,
        ValidationRuleLevel.Error => MessageType.Error,
        _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
    };
}
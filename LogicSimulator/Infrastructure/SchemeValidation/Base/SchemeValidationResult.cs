using LogicSimulator.ViewModels.Common;

namespace LogicSimulator.Infrastructure.SchemeValidation.Base;

public abstract class SchemeValidationResult
{
    protected SchemeValidationResult(bool isValid, ValidationRuleLevel level)
    {
        IsValid = isValid;
        Level = level;
    }

    public ValidationRuleLevel Level { get; }

    public bool IsValid { get; }

    public bool IsCritical => !IsValid && Level >= ValidationRuleLevel.Error;

    public IEnumerable<OutputMessageViewModel> Messages { get; protected set; } = [];
}
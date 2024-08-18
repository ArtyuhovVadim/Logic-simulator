namespace LogicSimulator.Infrastructure.SchemeValidation.Base;

public interface ISchemeValidationRule
{
    ValidationRuleLevel Level { get; }

    SchemeValidationResult Validate(ValidationContext context);
}
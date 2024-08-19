using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.Logic.Gates;
using LogicSimulator.Models.MessageSources;
using LogicSimulator.ViewModels.Common;

namespace LogicSimulator.Infrastructure.SchemeValidation;

public class SchemeWithoutOutputGatesValidationRule : ISchemeValidationRule
{
    public ValidationRuleLevel Level => ValidationRuleLevel.Warning;

    public SchemeValidationResult Validate(ValidationContext context)
    {
        var isValid = context.PreprocessedScheme.Gates.OfType<OutputGateModel>().Any();
        return new Result(context, isValid, Level);
    }

    public class Result : SchemeValidationResult
    {
        public Result(ValidationContext context, bool isValid, ValidationRuleLevel level) : base(isValid, level)
        {
            Messages = [new OutputMessageViewModel("Схема не имеет выходов.", Level, new DocumentMessageSource(context.Bus, context.Scheme))];
        }
    }
}

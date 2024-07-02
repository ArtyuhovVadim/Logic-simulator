using MathExpressionParser;

namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public abstract class BaseNumberPropertyViewModel : SinglePropertyViewModel
{
    protected static readonly MathParser Parser = MathParserBuilder
                                                  .Create()
                                                  .WithDefaultFunctions()
                                                  .WithDefaultConstants()
                                                  .WithConstant("nan", double.NaN).Build();

    #region IsValueUndefined

    private bool _isValueUndefined;

    public bool IsValueUndefined
    {
        get => _isValueUndefined;
        set => Set(ref _isValueUndefined, value);
    }

    #endregion
}
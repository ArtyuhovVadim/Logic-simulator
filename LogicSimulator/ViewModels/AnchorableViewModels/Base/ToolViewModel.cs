namespace LogicSimulator.ViewModels.AnchorableViewModels.Base;

public abstract class ToolViewModel : AnchorableViewModel
{
    #region IsVisible

    private bool _isVisible;

    public bool IsVisible
    {
        get => _isVisible;
        set => Set(ref _isVisible, value);
    }

    #endregion
}
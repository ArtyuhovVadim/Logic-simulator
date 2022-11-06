using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.AnchorableViewModels.Base;

public abstract class BaseAnchorableViewModel : BindableBase
{
    #region Name

    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    #endregion

    #region IsVisible

    private bool _isVisible;

    public bool IsVisible
    {
        get => _isVisible;
        set => Set(ref _isVisible, value);
    }

    #endregion
}
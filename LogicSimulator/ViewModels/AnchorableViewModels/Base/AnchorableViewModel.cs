using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.AnchorableViewModels.Base;

public abstract class AnchorableViewModel : BindableBase
{
    #region Title

    private string _title = string.Empty;

    public virtual string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    #endregion

    #region IsSelected

    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set => Set(ref _isSelected, value);
    }

    #endregion

    #region IsActive

    private bool _isActive;

    public bool IsActive
    {
        get => _isActive;
        set => Set(ref _isActive, value);
    }

    #endregion

    #region ContentId

    private string _contentId;

    public string ContentId
    {
        get => _contentId;
        set => Set(ref _contentId, value);
    }

    #endregion
}
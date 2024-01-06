using WpfExtensions.Mvvm;

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

    #region IconSource

    private Uri? _iconSource;

    public Uri? IconSource
    {
        get => _iconSource;
        set => Set(ref _iconSource, value);
    }

    #endregion

    #region IsSelected

    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (!Set(ref _isSelected, value)) return;

            if (value) OnSelected();
            else OnUnselected();
        }
    }

    #endregion

    #region IsActive

    private bool _isActive;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (!Set(ref _isActive, value)) return;

            if (value) OnActivated();
            else OnDeactivated();
        }
    }

    #endregion

    #region ContentId

    private string _contentId = string.Empty;

    public string ContentId
    {
        get => _contentId;
        set => Set(ref _contentId, value);
    }

    #endregion

    protected virtual void OnActivated() { }

    protected virtual void OnDeactivated() { }

    protected virtual void OnSelected() { }

    protected virtual void OnUnselected() { }
}
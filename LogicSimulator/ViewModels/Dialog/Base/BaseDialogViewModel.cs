using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.Dialog.Base;

public abstract class BaseDialogViewModel : BindableBase
{
    public event Action? Completed;

    #region Title

    private string _title = string.Empty;

    public string Title
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

    protected void OnCompleted() => Completed?.Invoke();
}
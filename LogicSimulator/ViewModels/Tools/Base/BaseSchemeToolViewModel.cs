using WpfExtensions.Mvvm;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.Tools.Base;

public abstract class BaseSchemeToolViewModel : BindableBase
{
    public event Action<BaseSchemeToolViewModel> ToolSelected;

    protected BaseSchemeToolViewModel(string name) => _name = name;

    #region Name

    private string _name;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
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

    #region SelectedCommand

    private ICommand _selectedCommand;

    public ICommand SelectedCommand => _selectedCommand ??= new LambdaCommand(OnSelected);

    #endregion

    protected virtual void OnSelected() => ToolSelected?.Invoke(this);
}
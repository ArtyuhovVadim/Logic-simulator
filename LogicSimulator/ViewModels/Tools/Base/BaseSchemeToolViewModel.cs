using WpfExtensions.Mvvm;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.Tools.Base;

public abstract class BaseSchemeToolViewModel : BindableBase
{
    public event Action<BaseSchemeToolViewModel>? ToolSelected;

    #region Name

    private string _name = string.Empty;

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
        set
        {
            if (!Set(ref _isActive, value)) return;

            if (value) OnActivated();
            else OnDeactivated();
        }
    }

    #endregion

    #region SelectedCommand

    private ICommand? _selectedCommand;

    public ICommand SelectedCommand => _selectedCommand ??= new LambdaCommand(OnSelected);

    #endregion

    protected virtual void OnSelected() => ToolSelected?.Invoke(this);

    protected virtual void OnActivated() { }

    protected virtual void OnDeactivated() { }
}
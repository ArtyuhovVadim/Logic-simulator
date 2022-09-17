using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public class PropertiesViewModel : BindableBase
{
    #region Name

    private string _name = "Свойства";

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    #endregion

    #region CurrentEditorViewModel

    private BaseEditorViewModel _currentEditorViewModel;

    public BaseEditorViewModel CurrentEditorViewModel
    {
        get => _currentEditorViewModel;
        set => Set(ref _currentEditorViewModel, value);
    }

    #endregion
}
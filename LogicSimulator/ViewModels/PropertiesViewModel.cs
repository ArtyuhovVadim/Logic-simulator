using LogicSimulator.ViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base;

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

    private AbstractEditorViewModel _currentEditorViewModel;

    public AbstractEditorViewModel CurrentEditorViewModel
    {
        get => _currentEditorViewModel;
        set => Set(ref _currentEditorViewModel, value);
    }

    #endregion
}
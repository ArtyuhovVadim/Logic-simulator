using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.ViewModels.AnchorableViewModels;

public class PropertiesViewModel : BaseAnchorableViewModel
{
    public PropertiesViewModel()
    {
        Name = "Свойства";
    }

    #region CurrentEditorViewModel

    private AbstractEditorViewModel _currentEditorViewModel;

    public AbstractEditorViewModel CurrentEditorViewModel
    {
        get => _currentEditorViewModel;
        set => Set(ref _currentEditorViewModel, value);
    }

    #endregion
}
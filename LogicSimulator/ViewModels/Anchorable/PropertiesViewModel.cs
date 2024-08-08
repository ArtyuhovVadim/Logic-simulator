using LogicSimulator.ViewModels.Anchorable.Base;
using LogicSimulator.ViewModels.Editors.Base;

namespace LogicSimulator.ViewModels.Anchorable;

public class PropertiesViewModel : ToolViewModel
{
    public override string Title => "Свойства";

    #region CurrentEditorViewModel

    private EditorViewModel? _currentEditorViewModel;

    public EditorViewModel? CurrentEditorViewModel
    {
        get => _currentEditorViewModel;
        set => Set(ref _currentEditorViewModel, value);
    }

    #endregion
}
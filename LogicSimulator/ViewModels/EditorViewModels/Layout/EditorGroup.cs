using System.Collections.ObjectModel;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorGroup : BindableBase
{
    private string _name;
    private ObservableCollection<EditorRow> _editorRows = new();

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    public ObservableCollection<EditorRow> EditorRows
    {
        get => _editorRows;
        set => Set(ref _editorRows, value);
    }
}
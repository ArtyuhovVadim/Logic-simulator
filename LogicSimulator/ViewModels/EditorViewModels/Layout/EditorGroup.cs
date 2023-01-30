using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorGroup
{
    public string Name { get; set; }

    public List<EditorRow> EditorRows { get; set; } = new();

    public EditorViewModel EditorViewModel { get; set; }

    public void PropertyChange(string propName)
    {
        foreach (var editorRow in EditorRows)
        {
            editorRow.PropertyChange(propName);
        }
    }
}
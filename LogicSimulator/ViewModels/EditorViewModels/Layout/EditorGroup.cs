using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorGroup
{
    public string Name { get; set; } = string.Empty;

    public List<EditorRow> EditorRows { get; set; } = [];

    public EditorViewModel EditorViewModel { get; set; } = null!;

    public void PropertyChange(string propName)
    {
        foreach (var editorRow in EditorRows)
        {
            editorRow.PropertyChange(propName);
        }
    }

    public void StartEdit()
    {
        foreach (var editorRow in EditorRows)
        {
            editorRow.StartEdit();
        }
    }

    public void EndEdit()
    {
        foreach (var editorRow in EditorRows)
        {
            editorRow.EndEdit();
        }
    }
}
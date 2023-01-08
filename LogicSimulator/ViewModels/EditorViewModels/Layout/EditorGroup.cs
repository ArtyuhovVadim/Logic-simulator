using System.Collections.Generic;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorGroup
{
    public string Name { get; }

    public IEnumerable<EditorRow> EditorRows { get; }

    public EditorGroup(string name, IEnumerable<EditorRow> editorRows)
    {
        Name = name;
        EditorRows = editorRows;
    }
}
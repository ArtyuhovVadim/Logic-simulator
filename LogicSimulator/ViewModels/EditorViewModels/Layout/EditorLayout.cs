using System.Collections.Generic;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorLayout
{
    public string ObjectName { get; }

    public IEnumerable<EditorGroup> Groups { get; }

    public EditorLayout(string objectName, IEnumerable<EditorGroup> groups)
    {
        ObjectName = objectName;
        Groups = groups;
    }
}
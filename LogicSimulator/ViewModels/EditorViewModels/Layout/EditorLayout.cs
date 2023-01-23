using System.Collections.Generic;
using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorLayout
{
    public string ObjectName { get; set; }

    public List<EditorGroup> Groups { get; set; } = new();

    public EditorViewModel EditorViewModel { get; set; }

    public void PropertyChange(string propName)
    {
        foreach (var group in Groups)
        {
            group.PropertyChange(propName);
        }
    }
}
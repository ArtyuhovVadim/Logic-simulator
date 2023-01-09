using System.Collections.Generic;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorLayout
{
    public string ObjectName { get; set; }

    public List<EditorGroup> Groups { get; set; } = new();
}
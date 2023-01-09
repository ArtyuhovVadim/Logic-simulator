using System.Collections.Generic;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorLayout : BindableBase
{
    public string ObjectName { get; set; }

    public List<EditorGroup> Groups { get; set; } = new();
}
using System.Collections.Generic;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorGroup : BindableBase
{
    public string Name { get; set; }

    public List<EditorRow> EditorRows { get; set; } = new();
}
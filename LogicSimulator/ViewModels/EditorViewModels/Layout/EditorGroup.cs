using System.Collections.Generic;
using System.Linq;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorGroup
{
    public string Name { get; set; }

    public List<EditorRow> EditorRows { get; set; } = new();
}
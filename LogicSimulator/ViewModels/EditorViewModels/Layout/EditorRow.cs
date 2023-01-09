using System.Collections.Generic;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorRow
{
    public string Name { get; set; } = string.Empty;

    public List<ObjectProperty> ObjectProperties { get; set; } = new();
}
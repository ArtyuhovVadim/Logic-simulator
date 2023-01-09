using System.Collections.Generic;
using System.Windows;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorRow : BindableBase
{
    public string Name { get; set; } = string.Empty;

    public List<ObjectProperty> ObjectProperties { get; set; } = new();

    public List<GridLength> Layout { get; set; } = new();
}
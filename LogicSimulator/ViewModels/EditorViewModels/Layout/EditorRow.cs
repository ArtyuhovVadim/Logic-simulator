using System.Collections.Generic;
using System.Windows;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorRow
{
    public string Name { get; set; } = string.Empty;

    public List<PropertyViewModel> ObjectProperties { get; set; } = new();

    public List<GridLength> Layout { get; set; } = new();

    public void PropertyChange(string propName)
    {
        foreach (var objectProperty in ObjectProperties)      
        {
            objectProperty.ProvidePropertyChanged(propName);
        }
    }
}
using LogicSimulator.ViewModels.EditorViewModels.Base;
using System.Windows;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorRow
{
    public string Name { get; set; } = string.Empty;

    public List<PropertyViewModel> ObjectProperties { get; set; } = new();

    public List<GridLength> Layout { get; set; } = new();

    public EditorViewModel EditorViewModel { get; set; }

    public void PropertyChange(string propName)
    {
        foreach (var objectProperty in ObjectProperties)      
        {
            objectProperty.ProvidePropertyChanged(propName);
        }
    }
}
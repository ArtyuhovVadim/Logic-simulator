using LogicSimulator.ViewModels.EditorViewModels.Base;
using System.Windows;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorRow
{
    public string Name { get; set; } = string.Empty;

    public List<PropertyViewModel> ObjectProperties { get; set; } = [];

    public List<GridLength> Layout { get; set; } = [];

    public EditorViewModel EditorViewModel { get; set; } = null!;

    public void PropertyChange(string propName)
    {
        foreach (var objectProperty in ObjectProperties)
        {
            objectProperty.ProvidePropertyChanged(propName);
        }
    }

    public void StartEdit()
    {
        foreach (var objectProperty in ObjectProperties)
        {
            objectProperty.StartEdit();
        }
    }

    public void EndEdit()
    {
        foreach (var objectProperty in ObjectProperties)
        {
            objectProperty.EndEdit();
        }
    }
}
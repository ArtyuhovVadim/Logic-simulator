using System.Windows;
using System.Windows.Controls;
using SharpDX;
using LogicSimulator.ViewModels.EditorViewModels.Layout;

namespace LogicSimulator.Infrastructure;

public class EditorPropertiesDataTemplateSelector : DataTemplateSelector
{
    //TODO: Refactor this!!!
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is not PropertyViewModel prop) return null;

        if (prop.PropertyType == typeof(float))
        {
            return ((FrameworkElement)container).FindResource("NumberBox") as DataTemplate;
        }
        if (prop.PropertyType == typeof(Vector2))
        {
            return ((FrameworkElement)container).FindResource("Vector2Box") as DataTemplate;
        }
        if (prop.PropertyType == typeof(Color4))
        {
            return ((FrameworkElement)container).FindResource("ColorPicker") as DataTemplate;
        }
        if (prop.PropertyType == typeof(bool))
        {
            return ((FrameworkElement)container).FindResource("CheckBoxEx") as DataTemplate;
        }

        return null;
    }
}
using LogicSimulator.ViewModels.EditorViewModels;
using System.Windows;
using System.Windows.Controls;
using SharpDX;

namespace LogicSimulator.Infrastructure;

public class EditorPropertiesDataTemplateSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is not ObjectPropertyViewModel prop) return null;

        if (prop.ObjectPropertyType == typeof(float))
        {
            return ((FrameworkElement)container).FindResource("NumberBox") as DataTemplate;
        }
        if (prop.ObjectPropertyType == typeof(Vector2))
        {
            return ((FrameworkElement)container).FindResource("Vector2Box") as DataTemplate;
        }
        if (prop.ObjectPropertyType == typeof(Color4))
        {
            return ((FrameworkElement)container).FindResource("ColorPicker") as DataTemplate;
        }
        if (prop.ObjectPropertyType == typeof(bool))
        {
            return ((FrameworkElement)container).FindResource("CheckBoxEx") as DataTemplate;
        }

        return null;
    }
}
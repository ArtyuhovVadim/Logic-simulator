using System.Windows;
using System.Windows.Controls;
using SharpDX;
using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.Infrastructure;

public class EditorPropertiesDataTemplateSelector : DataTemplateSelector
{
    //TODO: Refactor this!!!
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is not PropertyViewModel prop) return null;

        if (prop.PropertyPropertyType == typeof(float))
        {
            return ((FrameworkElement)container).FindResource("NumberBox") as DataTemplate;
        }
        if (prop.PropertyPropertyType == typeof(Vector2))
        {
            return ((FrameworkElement)container).FindResource("Vector2Box") as DataTemplate;
        }
        if (prop.PropertyPropertyType == typeof(Color4))
        {
            return ((FrameworkElement)container).FindResource("ColorPicker") as DataTemplate;
        }
        if (prop.PropertyPropertyType == typeof(bool))
        {
            return ((FrameworkElement)container).FindResource("CheckBoxEx") as DataTemplate;
        }

        return null;
    }
}
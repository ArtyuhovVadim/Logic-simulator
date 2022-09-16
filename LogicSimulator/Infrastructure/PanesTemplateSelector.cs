using System.Windows;
using System.Windows.Controls;
using LogicSimulator.ViewModels;

namespace LogicSimulator.Infrastructure;

public class PanesTemplateSelector : DataTemplateSelector
{
    public DataTemplate SchemeViewTemplate { get; set; }

    public DataTemplate PropertiesViewTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container) => item switch
    {
        SchemeViewModel => SchemeViewTemplate,
        PropertiesViewModel => PropertiesViewTemplate,
        _ => base.SelectTemplate(item, container)
    };
}
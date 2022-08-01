using System.Windows;
using System.Windows.Controls;
using LogicSimulator.ViewModels;

namespace LogicSimulator.Infrastructure;

public class PanesTemplateSelector : DataTemplateSelector
{
    public DataTemplate SchemeViewTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is SchemeViewModel)
            return SchemeViewTemplate;

        return base.SelectTemplate(item, container);
    }
}
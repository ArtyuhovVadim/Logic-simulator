using System.Windows;
using System.Windows.Controls;
using LogicSimulator.ViewModels;

namespace LogicSimulator.Infrastructure;

public class PanesStyleSelector : StyleSelector
{
    public Style SchemeViewStyle { get; set; }

    public override Style SelectStyle(object item, DependencyObject container)
    {
        if (item is SchemeViewModel)
            return SchemeViewStyle;

        return base.SelectStyle(item, container);
    }
}
using System.Windows;
using System.Windows.Controls;
using LogicSimulator.ViewModels;

namespace LogicSimulator.Infrastructure;

public class PanesStyleSelector : StyleSelector
{
    public Style SchemeViewStyle { get; set; }

    public Style PropertiesViewStyle { get; set; }

    public override Style SelectStyle(object item, DependencyObject container) => item switch
    {
        SchemeViewModel => SchemeViewStyle,
        PropertiesViewModel => PropertiesViewStyle,
        _ => base.SelectStyle(item, container)
    };
}
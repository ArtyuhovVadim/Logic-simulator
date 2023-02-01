using System.Windows;
using System.Windows.Controls;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;

namespace LogicSimulator.Infrastructure;

public class PanesStyleSelector : StyleSelector
{
    public Style ToolViewStyle { get; set; }

    public Style DocumentViewStyle { get; set; }

    public override Style SelectStyle(object item, DependencyObject container) => item switch
    {
        ToolViewModel => ToolViewStyle,
        DocumentViewModel => DocumentViewStyle,
        _ => base.SelectStyle(item, container)
    };
}
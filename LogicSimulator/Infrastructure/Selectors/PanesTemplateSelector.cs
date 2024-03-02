using System.Windows;
using System.Windows.Controls;
using LogicSimulator.ViewModels.AnchorableViewModels;

namespace LogicSimulator.Infrastructure.Selectors;

public class PanesTemplateSelector : DataTemplateSelector
{
    public DataTemplate SchemeViewTemplate { get; set; } = null!;

    public DataTemplate PropertiesViewTemplate { get; set; } = null!;

    public DataTemplate ProjectExplorerViewTemplate { get; set; } = null!;

    public DataTemplate MessagesOutputViewTemplate { get; set; } = null!;

    public override DataTemplate SelectTemplate(object? item, DependencyObject container) => item switch
    {
        SchemeViewModel => SchemeViewTemplate,
        PropertiesViewModel => PropertiesViewTemplate, 
        ProjectExplorerViewModel => ProjectExplorerViewTemplate,
        MessagesOutputViewModel => MessagesOutputViewTemplate,
        _ => base.SelectTemplate(item, container)!
    };
}
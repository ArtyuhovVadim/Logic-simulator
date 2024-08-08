using System.Windows;
using System.Windows.Controls;
using LogicSimulator.ViewModels.Logic;

namespace LogicSimulator.Infrastructure.Selectors;

public class PortViewTemplateSelector : DataTemplateSelector
{
    public DataTemplate PortDataTemplate { get; set; } = null!;

    public override DataTemplate SelectTemplate(object? item, DependencyObject container) => item switch
    {
        PortViewModel => PortDataTemplate,
        _ => throw new NotSupportedException()
    };
}
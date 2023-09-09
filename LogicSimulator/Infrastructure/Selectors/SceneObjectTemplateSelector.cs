using System.Windows.Controls;
using System.Windows;
using LogicSimulator.ViewModels.ObjectViewModels;

namespace LogicSimulator.Infrastructure.Selectors;

public class SceneObjectTemplateSelector : DataTemplateSelector
{
    public DataTemplate RectangleDataTemplate { get; set; } = null!;

    public override DataTemplate SelectTemplate(object item, DependencyObject container) => item switch
    {
        RectangleViewModel => RectangleDataTemplate,
        _ => base.SelectTemplate(item, container)!,
    };
}
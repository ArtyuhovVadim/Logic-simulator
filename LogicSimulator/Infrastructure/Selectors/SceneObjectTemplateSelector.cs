using System.Windows.Controls;
using System.Windows;
using LogicSimulator.ViewModels.ObjectViewModels;

namespace LogicSimulator.Infrastructure.Selectors;

public class SceneObjectTemplateSelector : DataTemplateSelector
{
    public DataTemplate RectangleDataTemplate { get; set; } = null!;

    public DataTemplate RoundedRectangleDataTemplate { get; set; } = null!;

    public DataTemplate ArcDataTemplate { get; set; } = null!;

    public DataTemplate TextBlockDataTemplate { get; set; } = null!;

    public DataTemplate EllipseDataTemplate { get; set; } = null!;

    public DataTemplate BezierCurveDataTemplate { get; set; } = null!;

    public DataTemplate UnknownObjectDataTemplate { get; set; } = null!;

    public override DataTemplate SelectTemplate(object item, DependencyObject container) => item switch
    {
        RoundedRectangleViewModel => UnknownObjectDataTemplate,
        RectangleViewModel => RectangleDataTemplate,
        ArcViewModel => UnknownObjectDataTemplate,
        TextBlockViewModel => UnknownObjectDataTemplate,
        EllipseViewModel => UnknownObjectDataTemplate,
        BezierCurveViewModel => UnknownObjectDataTemplate,
        _ => UnknownObjectDataTemplate,
    };
}
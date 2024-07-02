using System.Windows.Controls;
using System.Windows;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Gates;

namespace LogicSimulator.Infrastructure.Selectors;

public class SceneObjectTemplateSelector : DataTemplateSelector
{
    public DataTemplate WireDataTemplate { get; set; } = null!;

    public DataTemplate InputGateDataTemplate { get; set; } = null!;
    
    public DataTemplate OutputGateDataTemplate { get; set; } = null!;

    public DataTemplate AndGateDataTemplate { get; set; } = null!;

    public DataTemplate RectangleDataTemplate { get; set; } = null!;

    public DataTemplate RoundedRectangleDataTemplate { get; set; } = null!;

    public DataTemplate ArcDataTemplate { get; set; } = null!;

    public DataTemplate TextBlockDataTemplate { get; set; } = null!;

    public DataTemplate EllipseDataTemplate { get; set; } = null!;

    public DataTemplate BezierCurveDataTemplate { get; set; } = null!;

    public DataTemplate LineDataTemplate { get; set; } = null!;

    public DataTemplate PathDataTemplate { get; set; } = null!;

    public DataTemplate UnknownObjectDataTemplate { get; set; } = null!;

    public override DataTemplate SelectTemplate(object? item, DependencyObject container) => item switch
    {
        WireViewModel => WireDataTemplate,
        InputGateViewModel => InputGateDataTemplate,
        OutputGateViewModel => OutputGateDataTemplate,
        AndGateViewModel => AndGateDataTemplate,
        RoundedRectangleViewModel => RoundedRectangleDataTemplate,
        RectangleViewModel => RectangleDataTemplate,
        EllipseViewModel => EllipseDataTemplate,
        BezierCurveViewModel => BezierCurveDataTemplate,
        ArcViewModel => ArcDataTemplate,
        TextBlockViewModel => TextBlockDataTemplate,
        LineViewModel => LineDataTemplate,
        PathViewModel => PathDataTemplate,
        _ => UnknownObjectDataTemplate,
    };
}
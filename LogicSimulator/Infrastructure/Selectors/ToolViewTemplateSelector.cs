using System.Windows;
using System.Windows.Controls;
using LogicSimulator.ViewModels.Logic;
using LogicSimulator.ViewModels.Logic.Gates;
using LogicSimulator.ViewModels.Objects;
using LogicSimulator.ViewModels.Tools;

namespace LogicSimulator.Infrastructure.Selectors;

public class ToolViewTemplateSelector : DataTemplateSelector
{
    public DataTemplate DefaultToolTemplate { get; set; } = null!;

    public DataTemplate SelectionToolTemplate { get; set; } = null!;

    public DataTemplate DragToolTemplate { get; set; } = null!;

    public DataTemplate RectangleSelectionToolSelectionTemplate { get; set; } = null!;

    public DataTemplate NodeDragToolTemplate { get; set; } = null!;

    public DataTemplate RectanglePlacingToolTemplate { get; set; } = null!;

    public DataTemplate RoundedRectanglePlacingToolTemplate { get; set; } = null!;

    public DataTemplate EllipsePlacingToolTemplate { get; set; } = null!;

    public DataTemplate ArcPlacingToolTemplate { get; set; } = null!;

    public DataTemplate LinePlacingToolTemplate { get; set; } = null!;

    public DataTemplate BezierCurvePlacingToolTemplate { get; set; } = null!;

    public DataTemplate PathPlacingToolTemplate { get; set; } = null!;

    public DataTemplate TextPlacingToolTemplate { get; set; } = null!;

    public DataTemplate InputGatePlacingToolTemplate { get; set; } = null!;

    public DataTemplate OutputGatePlacingToolTemplate { get; set; } = null!;

    public DataTemplate AndGatePlacingToolTemplate { get; set; } = null!;

    public DataTemplate WirePlacingToolTemplate { get; set; } = null!;

    public override DataTemplate SelectTemplate(object? item, DependencyObject container) => item switch
    {
        SelectionToolViewModel => SelectionToolTemplate,
        DragToolViewModel => DragToolTemplate,
        RectangleSelectionToolViewModel => RectangleSelectionToolSelectionTemplate,
        NodeDragToolViewModel => NodeDragToolTemplate,
        RectanglePlacingToolViewModel => RectanglePlacingToolTemplate,
        RoundedRectanglePlacingToolViewModel => RoundedRectanglePlacingToolTemplate,
        EllipsePlacingToolViewModel => EllipsePlacingToolTemplate,
        ArcPlacingToolViewModel => ArcPlacingToolTemplate,
        SegmentedObjectPlacingToolViewModel<LineViewModel> => LinePlacingToolTemplate,
        BezierCurvePlacingToolViewModel => BezierCurvePlacingToolTemplate,
        ObjectPlacingToolViewModel<PathViewModel> => PathPlacingToolTemplate,
        ObjectPlacingToolViewModel<TextBlockViewModel> => TextPlacingToolTemplate,
        ObjectPlacingToolViewModel<InputGateViewModel> => InputGatePlacingToolTemplate,
        ObjectPlacingToolViewModel<OutputGateViewModel> => OutputGatePlacingToolTemplate,
        ObjectPlacingToolViewModel<AndGateViewModel> => AndGatePlacingToolTemplate,
        SegmentedObjectPlacingToolViewModel<WireViewModel> => WirePlacingToolTemplate,
        _ => DefaultToolTemplate
    } ?? DefaultToolTemplate;
}
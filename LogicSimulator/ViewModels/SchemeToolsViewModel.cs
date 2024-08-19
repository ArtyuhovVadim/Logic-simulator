using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Input;
using LogicSimulator.Models.Objects;
using LogicSimulator.ViewModels.Anchorable;
using LogicSimulator.ViewModels.Logic;
using LogicSimulator.ViewModels.Logic.Gates;
using LogicSimulator.ViewModels.Objects;
using LogicSimulator.ViewModels.Tools;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class SchemeToolsViewModel : BindableBase
{
    private readonly IToolSwitcherService _toolSwitcherService;

    public SchemeToolsViewModel(SchemeViewModel scheme, IToolSwitcherService toolSwitcherService)
    {
        _toolSwitcherService = toolSwitcherService;

        SelectionTool = new SelectionToolViewModel(scheme) { Name = "Selection tool" };
        DragTool = new DragToolViewModel(scheme) { Name = "Drag tool" };
        NodeDragTool = new NodeDragToolViewModel(scheme) { Name = "Node drag tool" };
        RectangleSelectionTool = new RectangleSelectionToolViewModel(scheme) { Name = "Rectangle selection tool" };

        var rectanglePlacingToolViewModel = new RectanglePlacingToolViewModel(scheme) { Group = ToolGroup.BaseGeometryPlacing, Name = "Rectangle placing tool" };
        var roundedRectanglePlacingToolViewModel = new RoundedRectanglePlacingToolViewModel(scheme) { Group = ToolGroup.BaseGeometryPlacing, Name = "Rounded rectangle placing tool" };
        var ellipsePlacingToolViewModel = new EllipsePlacingToolViewModel(scheme) { Group = ToolGroup.BaseGeometryPlacing, Name = "Ellipse placing tool" };
        var arcPlacingToolViewModel = new ArcPlacingToolViewModel(scheme) { Group = ToolGroup.BaseGeometryPlacing, Name = "Arc placing tool" };
        var linePlacingToolViewModel = new SegmentedObjectPlacingToolViewModel<LineViewModel>(scheme) { Group = ToolGroup.BaseGeometryPlacing, Name = "Line placing tool" };
        var bezierCurvePlacingToolViewModel = new BezierCurvePlacingToolViewModel(scheme) { Group = ToolGroup.BaseGeometryPlacing, Name = "Bezier placing tool" };
        var pathPlacingToolViewModel = new ObjectPlacingToolViewModel<PathViewModel>(scheme, () => new PathViewModel(new PathModel { Geometry = "M 32 0 L 0 16 L 0 56 L 32 71 L 64 56 L 64 18 Z M 32 4 L 60 20 L 32 34 L 4 18 Z M 4 22 L 30 38 L 30 66 L 4 54 Z M 60 24 L 60 54 L 34 66 L 34 38 Z" })) { Group = ToolGroup.BaseGeometryPlacing, Name = "Path placing tool" };
        var textBlockPlacingToolViewModel = new ObjectPlacingToolViewModel<TextBlockViewModel>(scheme) { Group = ToolGroup.BaseGeometryPlacing, Name = "Text block placing tool" };

        var inputGatePlacingToolViewModel = new ObjectPlacingToolViewModel<InputGateViewModel>(scheme) { Group = ToolGroup.GatesPlacing, Name = "Input gate placing tool" };
        var outputGatePlacingToolViewModel = new ObjectPlacingToolViewModel<OutputGateViewModel>(scheme) { Group = ToolGroup.GatesPlacing, Name = "Output gate placing tool" };
        var andGatePlacingToolViewModel = new ObjectPlacingToolViewModel<AndGateViewModel>(scheme) { Group = ToolGroup.GatesPlacing, Name = "And gate placing tool" };

        var wirePlacingToolViewModel = new SegmentedObjectPlacingToolViewModel<WireViewModel>(scheme) { Group = ToolGroup.WirePlacing, Name = "Wire placing tool" };

        _toolSwitcherService.ToolChanged += (_, _) => OnPropertyChanged(nameof(CurrentTool));
        _toolSwitcherService.DefaultTool = SelectionTool;

        _toolSwitcherService.AddTools(
            SelectionTool,
            DragTool,
            RectangleSelectionTool,
            NodeDragTool,
            rectanglePlacingToolViewModel,
            roundedRectanglePlacingToolViewModel,
            ellipsePlacingToolViewModel,
            arcPlacingToolViewModel,
            linePlacingToolViewModel,
            bezierCurvePlacingToolViewModel,
            pathPlacingToolViewModel,
            textBlockPlacingToolViewModel,
            inputGatePlacingToolViewModel,
            outputGatePlacingToolViewModel,
            andGatePlacingToolViewModel,
            wirePlacingToolViewModel);

        _toolSwitcherService.SwitchToDefaultTool(false);

        SelectionTool.SelectedObjectsChanged += scheme.SelectedObjectsChanged;
        RectangleSelectionTool.SelectedObjectsChanged += scheme.SelectedObjectsChanged;
    }

    #region Tools

    public IEnumerable<ITool> Tools => _toolSwitcherService.Tools;

    #endregion

    #region CurrnetTool

    public ITool? CurrentTool
    {
        get => _toolSwitcherService.CurrentTool;
        set => _toolSwitcherService.SwitchTool(value!.GetType(), false);
    }

    #endregion

    #region IsDefaultToolSelected

    public bool IsDefaultToolSelected => CurrentTool == _toolSwitcherService.DefaultTool;

    #endregion

    #region DefaultTool

    public ITool DefaultTool => _toolSwitcherService.DefaultTool!;

    #endregion

    #region IsCurrentToolLocked

    public bool IsCurrentToolLocked
    {
        get => _toolSwitcherService.IsCurrentToolLocked;
        set => Set(_toolSwitcherService.IsCurrentToolLocked, value, _toolSwitcherService, (service, value) => service.IsCurrentToolLocked = value);
    }

    #endregion

    public SelectionToolViewModel SelectionTool { get; }

    public DragToolViewModel DragTool { get; }

    public NodeDragToolViewModel NodeDragTool { get; }

    public RectangleSelectionToolViewModel RectangleSelectionTool { get; }
}
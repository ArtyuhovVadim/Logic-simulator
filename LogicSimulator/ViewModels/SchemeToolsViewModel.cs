using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Gates;
using LogicSimulator.ViewModels.Tools;
using LogicSimulator.ViewModels.Tools.Base;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class SchemeToolsViewModel : BindableBase
{
    public SchemeToolsViewModel(SchemeViewModel scheme)
    {
        CurrentTool = SelectionTool;

        SelectionTool.Name = "Selection tool";
        SelectionTool.ToolSelected += OnToolSelected;

        DragTool.Name = "Drag tool";
        DragTool.ToolSelected += OnToolSelected;

        RectangleSelectionTool.Name = "Rectangle selection tool";
        RectangleSelectionTool.ToolSelected += OnToolSelected;

        NodeDragTool.Name = "Node drag tool";
        NodeDragTool.ToolSelected += OnToolSelected;

        RectanglePlacingTool = new RectanglePlacingToolViewModel(scheme) { Name = "Rectangle placing tool" };
        RectanglePlacingTool.ToolSelected += OnToolSelected;

        RoundedRectanglePlacingTool = new RoundedRectanglePlacingToolViewModel(scheme) { Name = "Rounded rectangle placing tool" };
        RoundedRectanglePlacingTool.ToolSelected += OnToolSelected;

        EllipsePlacingTool = new EllipsePlacingToolViewModel(scheme) { Name = "Ellipse placing tool" };
        EllipsePlacingTool.ToolSelected += OnToolSelected;

        ArcPlacingTool = new ArcPlacingToolViewModel(scheme) { Name = "Arc placing tool" };
        ArcPlacingTool.ToolSelected += OnToolSelected;

        LinePlacingTool = new LinePlacingToolViewModel(scheme) { Name = "Line placing tool" };
        LinePlacingTool.ToolSelected += OnToolSelected;

        BezierCurvePlacingTool = new BezierCurvePlacingToolViewModel(scheme) { Name = "Bezier curve placing tool" };
        BezierCurvePlacingTool.ToolSelected += OnToolSelected;

        TextPlacingTool = new ObjectPlacingToolViewModel<TextBlockViewModel>(scheme, () => new TextBlockViewModel(new TextBlockModel())) { Name = "Text placing tool" };
        TextPlacingTool.ToolSelected += OnToolSelected;

        AndGatePlacingTool = new ObjectPlacingToolViewModel<AndGateViewModel>(scheme, () => new AndGateViewModel(new AndGateModel())) { Name = "And gate placing tool" };
        AndGatePlacingTool.ToolSelected += OnToolSelected;
    }

    public SchemeSelectionToolViewModel DefaultTool => SelectionTool;

    #region IsDefaultToolSelected

    public bool IsDefaultToolSelected => CurrentTool == DefaultTool;

    #endregion

    #region CurrentTool

    private BaseSchemeToolViewModel? _currentTool;

    public BaseSchemeToolViewModel? CurrentTool
    {
        get => _currentTool;
        set
        {
            var tmp = _currentTool;

            if (!Set(ref _currentTool, value)) return;

            if (tmp is not null)
                tmp.IsActive = false;

            if (_currentTool is not null)
                _currentTool.IsActive = true;

            OnPropertyChanged(nameof(IsDefaultToolSelected));
        }
    }

    #endregion

    #region IsCurrentToolLocked

    private bool _isCurrentToolLocked;

    public bool IsCurrentToolLocked
    {
        get => _isCurrentToolLocked;
        set => Set(ref _isCurrentToolLocked, value);
    }

    #endregion

    #region SelectionTool

    public SchemeSelectionToolViewModel SelectionTool { get; } = new();

    #endregion

    #region DragTool

    public SchemeDragToolViewModel DragTool { get; } = new();

    #endregion

    #region RectangleSelectionTool

    public SchemeRectangleSelectionToolViewModel RectangleSelectionTool { get; } = new();

    #endregion

    #region NodeDragTool

    public SchemeNodeDragToolViewModel NodeDragTool { get; } = new();

    #endregion

    #region RectanglePlacingTool

    public RectanglePlacingToolViewModel RectanglePlacingTool { get; }

    #endregion

    #region RoundedRectanglePlacingTool

    public RoundedRectanglePlacingToolViewModel RoundedRectanglePlacingTool { get; }

    #endregion

    #region EllipsePlacingTool

    public EllipsePlacingToolViewModel EllipsePlacingTool { get; }

    #endregion

    #region ArcPlacingTool

    public ArcPlacingToolViewModel ArcPlacingTool { get; }

    #endregion

    #region LinePlacingTool

    public LinePlacingToolViewModel LinePlacingTool { get; }

    #endregion

    #region BezierCurvePlacingTool

    public BezierCurvePlacingToolViewModel BezierCurvePlacingTool { get; }

    #endregion

    #region TextPlacingTool

    public ObjectPlacingToolViewModel<TextBlockViewModel> TextPlacingTool { get; }

    #endregion

    #region AndGatePlacingTool

    public ObjectPlacingToolViewModel<AndGateViewModel> AndGatePlacingTool { get; }

    #endregion

    private void OnToolSelected(BaseSchemeToolViewModel tool)
    {
        if (IsCurrentToolLocked)
            return;

        CurrentTool = tool;
    }
}
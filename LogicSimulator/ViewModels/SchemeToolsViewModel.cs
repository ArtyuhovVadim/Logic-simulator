using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.Tools;
using LogicSimulator.ViewModels.Tools.Base;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class SchemeToolsViewModel : BindableBase
{
    private readonly SchemeViewModel _scheme;

    public SchemeToolsViewModel(SchemeViewModel scheme)
    {
        _scheme = scheme;

        CurrentTool = SelectionTool;

        SelectionTool.ToolSelected += OnToolSelected;
        DragTool.ToolSelected += OnToolSelected;
        RectangleSelectionTool.ToolSelected += OnToolSelected;
        NodeDragTool.ToolSelected += OnToolSelected;

        RectanglePlacingTool = new RectanglePlacingToolViewModel("Rectangle placing tool", _scheme);
        RectanglePlacingTool.ToolSelected += OnToolSelected;
    }

    public SchemeSelectionToolViewModel DefaultTool => SelectionTool;

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

    public SchemeSelectionToolViewModel SelectionTool { get; } = new("Selection tool");

    #endregion

    #region DragTool

    public SchemeDragToolViewModel DragTool { get; } = new("Drag tool");

    #endregion

    #region RectangleSelectionTool

    public SchemeRectangleSelectionToolViewModel RectangleSelectionTool { get; } = new("Rectangle selection tool");

    #endregion

    #region NodeDragTool

    public SchemeNodeDragToolViewModel NodeDragTool { get; } = new("Node drag tool");

    #endregion

    #region RectanglePlacingTool

    public RectanglePlacingToolViewModel RectanglePlacingTool { get; }

    #endregion

    private void OnToolSelected(BaseSchemeToolViewModel tool)
    {
        if (IsCurrentToolLocked)
            return;

        CurrentTool = tool;
    }
}
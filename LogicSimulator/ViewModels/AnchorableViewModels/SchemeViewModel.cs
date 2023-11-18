using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using LogicSimulator.ViewModels.Tools;
using LogicSimulator.ViewModels.Tools.Base;
using Microsoft.Extensions.DependencyInjection;
using SharpDX;

namespace LogicSimulator.ViewModels.AnchorableViewModels;

public class SchemeViewModel : DocumentViewModel
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly Scheme _scheme;

    private readonly IEditorSelectionService _editorSelectionService;

    public SchemeViewModel(Scheme scheme)
    {
        _scheme = scheme;
        _mainWindowViewModel = App.Host.Services.GetRequiredService<MainWindowViewModel>();
        _editorSelectionService = App.Host.Services.GetRequiredService<IEditorSelectionService>();
        Objects = _scheme.Objects;

        CurrentTool = SelectionTool;

        SelectionTool.ToolSelected += OnToolSelected;
        DragTool.ToolSelected += OnToolSelected;
    }

    #region Title

    public override string Title
    {
        get => _scheme.Name;
        set
        {
            _scheme.Name = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Objects

    private ObservableCollection<BaseObjectViewModel> _objects;

    public IEnumerable<BaseObjectViewModel> Objects
    {
        get => _objects;
        private set => Set(ref _objects, new ObservableCollection<BaseObjectViewModel>(value));
    }

    #endregion

    #region CurrentTool

    private BaseSchemeToolViewModel _currentTool;

    public BaseSchemeToolViewModel CurrentTool
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

    #region SelectionTool

    public SchemeSelectionToolViewModel SelectionTool { get; } = new("Selection tool");

    #endregion

    #region DragTool

    public SchemeDragToolViewModel DragTool { get; } = new("Drag tool");

    #endregion

    #region Scale

    private float _scale = 1f;

    public float Scale
    {
        get => _scale;
        set => Set(ref _scale, value);
    }

    #endregion

    #region MousePosition

    private Vector2 _mousePosition = Vector2.Zero;

    public Vector2 MousePosition
    {
        get => _mousePosition;
        set => Set(ref _mousePosition, value);
    }

    #endregion

    protected override void Close(object p) => _mainWindowViewModel.OpenedSchemes.Remove(this);

    private void OnToolSelected(BaseSchemeToolViewModel tool) => CurrentTool = tool;

    /*#region Tools

    private readonly TransformTool _transformTool = new();

    private readonly SelectionTool _selectionTool = new();
    private readonly RectangleSelectionTool _rectangleSelectionTool = new();
    private readonly DragTool _dragTool = new();
    private readonly NodeDragTool _nodeDragTool = new();

    private readonly RectanglePlacingTool _rectanglePlacingTool = new();
    private readonly EllipsePlacingTool _ellipsePlacingTool = new();
    private readonly TextBlockPlacingTool _textBlockPlacingTool = new();
    private readonly WirePlacingTool _wirePlacingTool = new();

    #endregion

    public SchemeViewModel(Scheme scheme)
    {
        _scheme = scheme;

        Objects = new ObservableCollection<BaseSceneObject>(_scheme.Objects);

        var tools = new List<BaseTool>()
        {
            _selectionTool,
            _rectangleSelectionTool,
            _dragTool,
            _nodeDragTool,

            _rectanglePlacingTool,
            _ellipsePlacingTool,
            _textBlockPlacingTool,
            _wirePlacingTool
        };

        ToolsController = new ToolsController(_selectionTool)
        {
            Tools = tools,
            AlwaysUpdatingTools = new[] { _transformTool }
        };

        ToolsController.SelectedObjectsChanged += OnSelectedObjectsChanged;

        _editorSelectionService = App.Host.Services.GetRequiredService<IEditorSelectionService>();
        _mainWindowViewModel = App.Host.Services.GetRequiredService<MainWindowViewModel>();
    }

    private void OnSelectedObjectsChanged()
    {
        _editorSelectionService.Select(this);
    }

    #region ToolsController

    private ToolsController _toolsController;

    public ToolsController ToolsController
    {
        get => _toolsController;
        set => Set(ref _toolsController, value);
    }

    #endregion*/
}
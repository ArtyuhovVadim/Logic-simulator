using System.Windows.Media;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using LogicSimulator.ViewModels.StatusViewModels;
using LogicSimulator.ViewModels.StatusViewModels.Base;
using LogicSimulator.ViewModels.Tools;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.AnchorableViewModels;

public class SchemeViewModel : DocumentViewModel
{
    private readonly DockingViewModel _dockingViewModel;
    private readonly Scheme _scheme;

    private readonly SchemeStatusViewModel _statusViewModel;

    private readonly IEditorSelectionService _editorSelectionService;

    public SchemeViewModel(Scheme scheme, DockingViewModel dockingViewModel, IEditorSelectionService editorSelectionService)
    {
        _scheme = scheme;
        _dockingViewModel = dockingViewModel;
        _editorSelectionService = editorSelectionService;
        Objects = _scheme.Objects;

        CurrentTool = SelectionTool;

        SelectionTool.ToolSelected += OnToolSelected;
        DragTool.ToolSelected += OnToolSelected;
        RectangleSelectionTool.ToolSelected += OnToolSelected;
        NodeDragTool.ToolSelected += OnToolSelected;

        _statusViewModel = new SchemeStatusViewModel(this);

        _objects.CollectionChanged += (_, _) => _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.ObjectsCount));

        IconSource = new Uri("pack://application:,,,/Resources/Icons/scheme-icon16x16.png");
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

    private ObservableCollection<BaseObjectViewModel> _objects = [];

    public IEnumerable<BaseObjectViewModel> Objects
    {
        get => _objects;
        private set => Set(ref _objects, new ObservableCollection<BaseObjectViewModel>(value));
    }

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
        }
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

    #region Scale

    private float _scale = 1f;

    public float Scale
    {
        get => _scale;
        set
        {
            if (Set(ref _scale, value))
            {
                _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.Scale));
            }
        }
    }

    #endregion

    #region Translation

    private Vector2 _translation = Vector2.Zero;

    public Vector2 Translation
    {
        get => _translation;
        set => Set(ref _translation, value);
    }

    #endregion

    #region MousePosition

    private Vector2 _mousePosition = Vector2.Zero;

    public Vector2 MousePosition
    {
        get => _mousePosition;
        set
        {
            if (Set(ref _mousePosition, value))
            {
                _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.MousePosition));
            }
        }
    }

    #endregion

    #region GridStep

    private float _gridStep = 25;

    public float GridStep
    {
        get => _gridStep;
        set => Set(ref _gridStep, value);
    }

    #endregion

    #region GridWidth

    private float _gridWidth = 2970;

    public float GridWidth
    {
        get => _gridWidth;
        set => Set(ref _gridWidth, value);
    }

    #endregion

    #region GridHeight

    private float _gridHeight = 2100;

    public float GridHeight
    {
        get => _gridHeight;
        set => Set(ref _gridHeight, value);
    }

    #endregion

    #region ObjectSelectedCommand

    private ICommand? _objectSelectedCommand;

    public ICommand ObjectSelectedCommand => _objectSelectedCommand ??= new LambdaCommand(_ =>
    {
        var selectedObjects = Objects.Where(x => x.IsSelected).ToArray();

        _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.SelectedObjectsCount));

        if (selectedObjects.Length == 0)
        {
            _editorSelectionService.SetSchemeEditor(this);
            return;
        }

        _editorSelectionService.SetObjectsEditor(selectedObjects);
    });

    #endregion

    public override BaseStatusViewModel StatusViewModel => _statusViewModel;

    protected override void OnDocumentActivated()
    {
        var selectedObjects = Objects.Where(x => x.IsSelected).ToArray();

        if (selectedObjects.Length == 0)
        {
            _editorSelectionService.SetSchemeEditor(this);
            return;
        }

        _editorSelectionService.SetObjectsEditor(selectedObjects);
    }

    protected override void OnDocumentDeactivated()
    {
        _editorSelectionService.SetEmptyEditor();
    }

    protected override void OnClose(object? p) => _dockingViewModel.RemoveDocumentViewModel(this);

    private void OnToolSelected(BaseSchemeToolViewModel tool) => CurrentTool = tool;
}
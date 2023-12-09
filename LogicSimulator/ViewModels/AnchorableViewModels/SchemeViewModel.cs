using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using LogicSimulator.ViewModels.Tools;
using LogicSimulator.ViewModels.Tools.Base;
using Microsoft.Extensions.DependencyInjection;
using SharpDX;
using WpfExtensions.Mvvm.Commands;

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
        RectangleSelectionTool.ToolSelected += OnToolSelected;
        NodeDragTool.ToolSelected += OnToolSelected;
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

    #region ObjectSelectedCommand

    private ICommand _objectSelectedCommand;

    public ICommand ObjectSelectedCommand => _objectSelectedCommand ??= new LambdaCommand(_ =>
    {
        _editorSelectionService.Select(this);
    });

    #endregion

    protected override void Close(object p) => _mainWindowViewModel.OpenedSchemes.Remove(this);

    private void OnToolSelected(BaseSchemeToolViewModel tool) => CurrentTool = tool;
}
using LogicSimulator.Core;
using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Models.Base;
using LogicSimulator.Shared;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using LogicSimulator.ViewModels.StatusViewModels;
using LogicSimulator.ViewModels.StatusViewModels.Base;
using Microsoft.Extensions.Logging;
using SharpDX;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.AnchorableViewModels;

public class SchemeViewModel : DocumentViewModel, IModelBased<Scheme>, ICloseable
{
    private readonly DockingViewModel _dockingViewModel;
    private readonly SchemeStatusViewModel _statusViewModel;

    private readonly IEditorSelectionService _editorSelectionService;
    private readonly ISchemeSimulatorService _schemeSimulatorService;
    private readonly ISchemeBuilderService _schemeBuilderService;
    private readonly IToolSwitcherService _toolSwitcherService;
    private readonly ILogger<SchemeViewModel> _logger;

    private List<BaseObjectViewModel> _selectedObjects = [];
    private LogicScheme? _currentScheme;
    private ObservableCollection<TimelineRowViewModel> _simulationResult = [];

    public SchemeViewModel(Scheme scheme,
                           DockingViewModel dockingViewModel,
                           IEditorSelectionService editorSelectionService,
                           ISchemeSimulatorService schemeSimulatorService,
                           ISchemeBuilderService schemeBuilderService,
                           IToolSwitcherService toolSwitcherService,
                           IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel> viewModelsFactory,
                           ILogger<SchemeViewModel> logger)
    {
        Model = scheme;
        _dockingViewModel = dockingViewModel;
        _editorSelectionService = editorSelectionService;
        _schemeSimulatorService = schemeSimulatorService;
        _schemeBuilderService = schemeBuilderService;
        _toolSwitcherService = toolSwitcherService;
        _logger = logger;

        _objects = new ObservableCollectionEx<BaseObjectViewModel, BaseObjectModel>(Model.Objects, viewModelsFactory.Create);
        _statusViewModel = new SchemeStatusViewModel(this);
        _objects.CollectionChanged += (_, _) => _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.ObjectsCount));

        ToolsViewModel = new SchemeToolsViewModel(this, _toolSwitcherService);
        ToolsViewModel.DragTool.GridStep = GridStep;

        IconSource = new Uri("pack://application:,,,/Resources/Icons/scheme-icon16x16.png");
        base.Title = Model.FileInfo?.Name ?? throw new InvalidOperationException();
    }

    public event Action? Closed;

    #region HitTester

    private IHitTester _hitTester = null!;

    public IHitTester HitTester
    {
        get => _hitTester;
        set => Set(ref _hitTester, value);
    }

    #endregion

    #region Model

    public Scheme Model { get; }

    #endregion

    #region ToolsViewModel
    
    public SchemeToolsViewModel ToolsViewModel { get; }

    #endregion

    #region Objects

    private readonly ObservableCollectionEx<BaseObjectViewModel, BaseObjectModel> _objects;

    public ObservableCollection<BaseObjectViewModel> Objects => _objects;

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

    private float _gridStep = 20;

    public float GridStep
    {
        get => _gridStep;
        set
        {
            if (Set(ref _gridStep, value))
            {
                ToolsViewModel.DragTool.GridStep = value;
            }
        }
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

    #region SelectedObjects

    public IReadOnlyList<BaseObjectViewModel> SelectedObjects => _selectedObjects;

    #endregion

    #region StatusViewModel

    public override BaseStatusViewModel StatusViewModel => _statusViewModel;

    #endregion

    #region ObjectSelectedCommand

    private ICommand? _objectSelectedCommand;

    public ICommand ObjectSelectedCommand => _objectSelectedCommand ??= new LambdaCommand(OnSelectedObjectsChanged);

    #endregion

    #region DeleteSelectedObjectsCommand

    private ICommand? _deleteSelectedObjectsCommand;

    public ICommand DeleteSelectedObjectsCommand => _deleteSelectedObjectsCommand ??= new LambdaCommand(() =>
    {
        foreach (var selectedObject in _selectedObjects)
        {
            Objects.Remove(selectedObject);
        }

        SelectedObjectsChanged();
    }, () => ToolsViewModel.IsDefaultToolSelected);

    #endregion

    #region SelectAllObjectsCommand

    private ICommand? _selectAllObjectsCommand;

    public ICommand SelectAllObjectsCommand => _selectAllObjectsCommand ??= new LambdaCommand(() =>
    {
        foreach (var obj in Objects)
        {
            obj.IsSelected = true;
        }

        SelectedObjectsChanged();
    }, () => ToolsViewModel.IsDefaultToolSelected);

    #endregion

    #region RotateSelectedObjectsClockwiseCommand

    private ICommand? _rotateSelectedObjectsClockwiseCommand;

    public ICommand RotateSelectedObjectsClockwiseCommand => _rotateSelectedObjectsClockwiseCommand ??= new LambdaCommand(() =>
    {
        foreach (var obj in _selectedObjects)
        {
            obj.RotateClockwise();
        }
    }, () => ToolsViewModel.IsDefaultToolSelected);

    #endregion

    #region RotateSelectedObjectsCounterclockwiseCommand

    private ICommand? _rotateSelectedObjectsCounterclockwiseCommand;

    public ICommand RotateSelectedObjectsCounterclockwiseCommand => _rotateSelectedObjectsCounterclockwiseCommand ??= new LambdaCommand(() =>
    {
        foreach (var obj in _selectedObjects)
        {
            obj.RotateCounterclockwise();
        }
    }, () => ToolsViewModel.IsDefaultToolSelected);

    #endregion

    #region StartSimulationCommand

    private ICommand? _startSimulationCommand;

    public ICommand StartSimulationCommand => _startSimulationCommand ??= new LambdaCommand(() =>
    {
        try
        {
            if (_schemeSimulatorService.State is SimulationState.Stopped)
            {
                _currentScheme = _schemeBuilderService.BuildFromViewModels(Objects);

                //TODO: Test
                foreach (var gate in _currentScheme.InputGates)
                    gate.State = SignalType.High;
                _currentScheme.InputGates.First().State = SignalType.Low;

                _schemeSimulatorService.StartSimulation(_currentScheme, new SimulatorSettings { IsPauseSupported = true, IsPausedOnStart = true, AdditionalSimulationTime = 10 });
            }
            else
            {
                _schemeSimulatorService.ResumeSimulation();
            }
        }
        catch (Exception e)
        {
            _schemeSimulatorService.StopSimulation();
            _logger.LogError("{e}", e);
        }
    }, () => _schemeSimulatorService.CanStart || _schemeSimulatorService.CanResume);

    #endregion

    #region PauseSimulationCommand

    private ICommand? _pauseSimulationCommand;

    public ICommand PauseSimulationCommand => _pauseSimulationCommand ??= new LambdaCommand(() =>
    {
        try
        {
            _schemeSimulatorService.PauseSimulation();
            _simulationResult = new ObservableCollection<TimelineRowViewModel>(_schemeSimulatorService.Result.Select(x => new TimelineRowViewModel(x.Value)));
            //TODO:
            _dockingViewModel.ToolViewModels.OfType<TimelineViewModel>().Single().Waves = _simulationResult;
        }
        catch (Exception e)
        {
            _schemeSimulatorService.StopSimulation();
            _logger.LogError("{e}", e);
        }
    }, () => _schemeSimulatorService.CanPause);

    #endregion

    #region NextSimulationStepCommand

    private ICommand? _nextSimulationStepCommand;

    public ICommand NextSimulationStepCommand => _nextSimulationStepCommand ??= new LambdaCommand(() =>
    {
        try
        {
            _schemeSimulatorService.SimulateNextStep();
            _simulationResult = new ObservableCollection<TimelineRowViewModel>(_schemeSimulatorService.Result.Select(x => new TimelineRowViewModel(x.Value)));
            //TODO:
            _dockingViewModel.ToolViewModels.OfType<TimelineViewModel>().Single().Waves = _simulationResult;
        }
        catch (Exception e)
        {
            _schemeSimulatorService.StopSimulation();
            _logger.LogError("{e}", e);
        }
    }, () => _schemeSimulatorService.State is SimulationState.Paused);

    #endregion

    #region StopSimulationCommandCommand

    private ICommand? _stopSimulationCommandCommand;

    public ICommand StopSimulationCommandCommand => _stopSimulationCommandCommand ??= new LambdaCommand(() =>
    {
        try
        {
            _schemeSimulatorService.StopSimulation();
            _simulationResult = new ObservableCollection<TimelineRowViewModel>(_schemeSimulatorService.Result.Select(x => new TimelineRowViewModel(x.Value)));
            //TODO:
            _dockingViewModel.ToolViewModels.OfType<TimelineViewModel>().Single().Waves = _simulationResult;
        }
        catch (Exception e)
        {
            _logger.LogError("{e}", e);
        }
    }, () => _schemeSimulatorService.CanStop);

    #endregion

    public void SelectedObjectsChanged() => OnSelectedObjectsChanged();

    protected override void OnDocumentActivated()
    {
        OnSelectedObjectsChanged();
        //TODO:
        _dockingViewModel.ToolViewModels.OfType<TimelineViewModel>().Single().Waves = _simulationResult;
    }

    protected override void OnDocumentDeactivated()
    {
        _editorSelectionService.SetEmptyEditor();
        //TODO:
        _dockingViewModel.ToolViewModels.OfType<TimelineViewModel>().Single().Waves = [];
    }

    protected override void OnClose(object? p)
    {
        _dockingViewModel.CloseDocumentViewModel(this);
        Closed?.Invoke();
    }

    private void OnSelectedObjectsChanged()
    {
        _selectedObjects = Objects.Where(x => x.IsSelected).ToList();

        _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.SelectedObjectsCount));

        if (_selectedObjects.Count == 0)
        {
            _editorSelectionService.SetSchemeEditor(this);
            return;
        }

        _editorSelectionService.SetObjectsEditor(_selectedObjects);

        OnPropertyChanged(nameof(SelectedObjects));
    }
}
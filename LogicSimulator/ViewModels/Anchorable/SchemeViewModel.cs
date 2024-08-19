using LogicSimulator.Core;
using LogicSimulator.Infrastructure.Collections;
using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Infrastructure.Messages;
using LogicSimulator.Infrastructure.SchemeValidation;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Models.Common;
using LogicSimulator.Models.Logic;
using LogicSimulator.Models.MessageSources;
using LogicSimulator.Models.Objects.Base;
using LogicSimulator.Models.Simulation;
using LogicSimulator.Shared.Models.HitTest;
using LogicSimulator.ViewModels.Anchorable.Base;
using LogicSimulator.ViewModels.Objects.Base;
using LogicSimulator.ViewModels.Status;
using LogicSimulator.ViewModels.Status.Base;
using Microsoft.Extensions.Logging;
using SharpDX;
using WpfExtensions.Mvvm.Commands;
using WpfExtensions.Mvvm.Messaging;

namespace LogicSimulator.ViewModels.Anchorable;

public class SchemeViewModel : DocumentViewModel, IModelBased<Scheme>, ICloseable
{
    private readonly SchemeStatusViewModel _statusViewModel;

    private readonly IEditorSelectionService _editorSelectionService;
    private readonly ISchemeSimulatorService _schemeSimulatorService;
    private readonly ISchemeBuilderService _schemeBuilderService;
    private readonly IToolSwitcherService _toolSwitcherService;
    private readonly IOutputMessagesService _outputMessagesService;
    private readonly IMessageBus _messageBus;
    private readonly ILogger<SchemeViewModel> _logger;

    private List<BaseObjectViewModel> _selectedObjects = [];
    private LogicScheme? _currentScheme;

    public SchemeViewModel(Scheme scheme,
                           IEditorSelectionService editorSelectionService,
                           ISchemeSimulatorService schemeSimulatorService,
                           ISchemeBuilderService schemeBuilderService,
                           IToolSwitcherService toolSwitcherService,
                           IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel> viewModelsFactory,
                           IOutputMessagesService outputMessagesService,
                           IMessageBus messageBus,
                           ILogger<SchemeViewModel> logger)
    {
        Model = scheme;
        _editorSelectionService = editorSelectionService;
        _schemeSimulatorService = schemeSimulatorService;
        _schemeBuilderService = schemeBuilderService;
        _toolSwitcherService = toolSwitcherService;
        _outputMessagesService = outputMessagesService;
        _messageBus = messageBus;
        _logger = logger;

        _objects = new ObservableCollectionEx<BaseObjectViewModel, BaseObjectModel>(Model.Objects, viewModelsFactory.Create);
        _statusViewModel = new SchemeStatusViewModel(this);
        _objects.CollectionChanged += (_, _) => _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.ObjectsCount));

        ToolsViewModel = new SchemeToolsViewModel(this, _toolSwitcherService);
        ToolsViewModel.DragTool.GridStep = GridStep;

        IconSource = new Uri("pack://application:,,,/Resources/Icons/scheme-icon16x16.png");
        base.Title = Model.FileInfo?.Name ?? throw new InvalidOperationException();

        _schemeSimulatorService.SimulationStateChanged += OnSimulationStateChanged;

        _schemeBuilderService.AddValidationRule(new GateNamesMustBeUniqueAndNotEmptyValidationRule());
        _schemeBuilderService.AddValidationRule(new PortMustBeConnectedValidationRule());
        _schemeBuilderService.AddValidationRule(new MoreThenOneOutputPortConnectedValidationRule());
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

    #region ViewportSize

    private Size2F _viewportSize;

    public Size2F ViewportSize
    {
        get => _viewportSize;
        set => Set(ref _viewportSize, value);
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
                ToolsViewModel.NodeDragTool.GridStep = value;
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

    public ICommand SelectAllObjectsCommand => _selectAllObjectsCommand ??= new LambdaCommand(SelectAllObjects, () => ToolsViewModel.IsDefaultToolSelected);

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
                _outputMessagesService.ClearMessages();

                var scheme = _schemeBuilderService.BuildFromSchemeViewModel(this);

                foreach (var validationResult in scheme.ValidationResults)
                {
                    if (!validationResult.IsValid)
                    {
                        foreach (var message in validationResult.Messages)
                        {
                            _outputMessagesService.AddMessage(message);
                        }
                    }
                }

                if (!scheme.IsValid)
                {
                    _outputMessagesService.AddErrorMessage("Обнаружены ошибки, симуляция не может быть запущена.", new DocumentMessageSource(_messageBus, this));
                    return;
                }

                _currentScheme = scheme;

                //TODO: Test
                foreach (var gate in _currentScheme.InputGateLogicModels)
                    gate.State = SignalType.High;
                _currentScheme.InputGateLogicModels.First().State = SignalType.Low;

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
            var oldState = _schemeSimulatorService.State;
            _schemeSimulatorService.SimulateNextStep();
            _messageBus.Send(new SimulationStateChangedMessage(this, oldState, _schemeSimulatorService.State, _schemeSimulatorService.GetSimulationResult()));
        }
        catch (Exception e)
        {
            _schemeSimulatorService.StopSimulation();
            _logger.LogError("{e}", e);
        }
    }, () => _schemeSimulatorService.State is SimulationState.Paused);

    #endregion

    #region StopSimulationCommandCommand

    private ICommand? _stopSimulationCommand;

    public ICommand StopSimulationCommand => _stopSimulationCommand ??= new LambdaCommand(() =>
    {
        try
        {
            _schemeSimulatorService.StopSimulation();
        }
        catch (Exception e)
        {
            _logger.LogError("{e}", e);
        }
    }, () => _schemeSimulatorService.CanStop);

    #endregion

    public Dictionary<string, PortSimulationResult> GetSimulationResult() => _schemeSimulatorService.GetSimulationResult();

    public void PanToObjectsAndSelect(ICollection<BaseObjectViewModel> objects)
    {
        var bounds = objects.Select(HitTester.GetFromContext).ToArray();

        if (bounds.Length == 0 || bounds.Any(x => x is null))
            return;

        var point = bounds.Length switch
        {
            1 => bounds[0]!.WorldBounds.Center,
            _ => bounds.Aggregate(bounds[0]!.WorldBounds, (rect, obj) => RectangleF.Union(rect, obj!.WorldBounds)).Center
        };

        SetViewportCenterPoint(point);

        DeselectAllObjects();
        foreach (var obj in objects)
            obj.IsSelected = true;
        SelectedObjectsChanged();
    }

    public void PanToObjectsAndSelect(ICollection<BaseObjectModel> objects)
    {
        var objectsMap = Objects.ToDictionary(x => x.Model);
        PanToObjectsAndSelect(objects.Select(obj => objectsMap[obj]).ToArray());
    }

    public void PanToObjectAndSelect(BaseObjectViewModel obj)
    {
        var hitTestable = HitTester.GetFromContext(obj);

        if (hitTestable is null)
            return;

        SetViewportCenterPoint(hitTestable.WorldBounds.Center);

        DeselectAllObjects();
        obj.IsSelected = true;
        SelectedObjectsChanged();
    }

    public void PanToObjectAndSelect(BaseObjectModel obj)
    {
        var viewModel = Objects.First(x => x.Model == obj);
        PanToObjectAndSelect(viewModel);
    }

    public void SetViewportCenterPoint(Vector2 point)
    {
        var viewportSize = new Vector2(ViewportSize.Width, ViewportSize.Height);
        Translation = viewportSize / 2f - point * Scale;
    }

    public void SelectAllObjects()
    {
        foreach (var obj in Objects)
            obj.IsSelected = true;

        SelectedObjectsChanged();
    }

    public void DeselectAllObjects()
    {
        foreach (var obj in Objects)
            obj.IsSelected = false;

        SelectedObjectsChanged();
    }

    public void SelectedObjectsChanged() => OnSelectedObjectsChanged();

    protected override void OnDocumentActivated()
    {
        _messageBus.Send(new DocumentActivatedMessage(this));
        OnSelectedObjectsChanged();
    }

    protected override void OnDocumentDeactivated()
    {
        _messageBus.Send(new DocumentDeactivatedMessage(this));
        _editorSelectionService.SetEmptyEditor();
    }

    protected override void OnClose()
    {
        _messageBus.Send(new DocumentClosingMessage(this));
        Closed?.Invoke();
    }

    private void OnSimulationStateChanged(SimulationState oldState, SimulationState newState) =>
        _messageBus.Send(new SimulationStateChangedMessage(this, oldState, newState, _schemeSimulatorService.GetSimulationResult()));

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
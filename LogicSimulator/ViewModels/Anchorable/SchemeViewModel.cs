using LogicSimulator.Core;
using LogicSimulator.Infrastructure.Collections;
using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Infrastructure.Messages;
using LogicSimulator.Infrastructure.SchemeValidation;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Models.Common;
using LogicSimulator.Models.Input;
using LogicSimulator.Models.Logic;
using LogicSimulator.Models.MessageSources;
using LogicSimulator.Models.Objects.Base;
using LogicSimulator.Models.Simulation;
using LogicSimulator.Shared.ExtensionMethods;
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
    private readonly IClipboardService _clipboardService;
    private readonly IMessageBus _messageBus;
    private readonly ILogger<SchemeViewModel> _logger;

    private ITool? _lastSelectedTool;
    private List<(Vector2 Offset, BaseObjectViewModel Object)> _cursorFollowingObjects = [];


    private LogicScheme? _currentScheme;

    public SchemeViewModel(Scheme scheme,
                           IEditorSelectionService editorSelectionService,
                           ISchemeSimulatorService schemeSimulatorService,
                           ISchemeBuilderService schemeBuilderService,
                           IToolSwitcherService toolSwitcherService,
                           IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel> viewModelsFactory,
                           IOutputMessagesService outputMessagesService,
                           IClipboardService clipboardService,
                           IMessageBus messageBus,
                           ILogger<SchemeViewModel> logger)
    {
        Model = scheme;
        _editorSelectionService = editorSelectionService;
        _schemeSimulatorService = schemeSimulatorService;
        _schemeBuilderService = schemeBuilderService;
        _toolSwitcherService = toolSwitcherService;
        _outputMessagesService = outputMessagesService;
        _clipboardService = clipboardService;
        _messageBus = messageBus;
        _logger = logger;

        _objects = new ObservableCollectionEx<BaseObjectViewModel, BaseObjectModel>(Model.Objects, viewModelsFactory.Create);
        _statusViewModel = new SchemeStatusViewModel(this);
        _objects.CollectionChanged += (_, _) => _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.ObjectsCount));

        ToolsViewModel = new SchemeToolsViewModel(this, _toolSwitcherService);
        ToolsViewModel.DragTool.GridStep = GridStep;
        ToolsViewModel.NodeDragTool.GridStep = GridStep;

        IconSource = new Uri("pack://application:,,,/Resources/Icons/scheme-icon16x16.png");

        _schemeSimulatorService.SimulationStateChanged += OnSimulationStateChanged;

        _schemeBuilderService.AddValidationRule(new GateNamesMustBeUniqueAndNotEmptyValidationRule());
        _schemeBuilderService.AddValidationRule(new PortNamesMustBeUniqueAndNotEmptyValidationRule());
        _schemeBuilderService.AddValidationRule(new PortMustBeConnectedValidationRule());
        _schemeBuilderService.AddValidationRule(new MoreThenOneOutputPortConnectedValidationRule());
        _schemeBuilderService.AddValidationRule(new WireMustBeConnectedToSomethingValidationRule());
        _schemeBuilderService.AddValidationRule(new OnlyInputPortsConnectedValidationRule());
        _schemeBuilderService.AddValidationRule(new OnlyOutputPortConnectedValidationRule());
        _schemeBuilderService.AddValidationRule(new SchemeWithoutInputGatesValidationRule());
        _schemeBuilderService.AddValidationRule(new SchemeWithoutOutputGatesValidationRule());
    }

    public event Action? Closed;

    #region Title

    public override string Title
    {
        get => Model.Name;
        set => Set(Model.Name, value, Model, (model, value) => model.Name = value);
    }

    #endregion

    #region Model

    public Scheme Model { get; }

    #endregion

    #region HitTester

    private IHitTester _hitTester = null!;

    public IHitTester HitTester
    {
        get => _hitTester;
        set => Set(ref _hitTester, value);
    }

    #endregion

    #region ToolsViewModel

    public SchemeToolsViewModel ToolsViewModel { get; }

    #endregion

    #region Objects

    private readonly ObservableCollectionEx<BaseObjectViewModel, BaseObjectModel> _objects;

    public ObservableCollection<BaseObjectViewModel> Objects => _objects;

    #endregion

    #region SelectedObjects

    public IReadOnlyList<BaseObjectViewModel> SelectedObjects => Objects.Where(x => x.IsSelected).ToList();

    #endregion

    #region HasSelectedObjects

    public bool HasSelectedObjects => Objects.Any(x => x.IsSelected);

    #endregion

    #region HasCursorFollowingObjects

    public bool HasCursorFollowingObjects => _cursorFollowingObjects.Count > 0;

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

    #region StatusViewModel

    public override BaseStatusViewModel StatusViewModel => _statusViewModel;

    #endregion

    #region DeleteSelectedObjectsCommand

    private ICommand? _deleteSelectedObjectsCommand;

    public ICommand DeleteSelectedObjectsCommand => _deleteSelectedObjectsCommand ??= new LambdaCommand(() =>
    {
        foreach (var selectedObject in SelectedObjects)
        {
            Objects.Remove(selectedObject);
        }

        SelectedObjectsChanged();
    }, () => ToolsViewModel.IsDefaultToolSelected && HasSelectedObjects);

    #endregion

    #region SelectAllObjectsCommand

    private ICommand? _selectAllObjectsCommand;

    public ICommand SelectAllObjectsCommand => _selectAllObjectsCommand ??= new LambdaCommand(SelectAllObjects, () => ToolsViewModel.IsDefaultToolSelected);

    #endregion

    #region CopyCommand

    private ICommand? _copyCommand;

    public ICommand CopyCommand => _copyCommand ??= new LambdaCommand(() =>
    {
        _clipboardService.Copy(SelectedObjects);
    }, () => ToolsViewModel.IsDefaultToolSelected && HasSelectedObjects);

    #endregion

    #region PasteCommand

    private ICommand? _pasteCommand;

    public ICommand PasteCommand => _pasteCommand ??= new LambdaCommand(() =>
    {
        var objects = _clipboardService.Paste();
        _objects.AddRange(objects);
        StartFollowCursor(objects);
        UpdateCursorFollowingObjectsLocation();
    }, () => ToolsViewModel.IsDefaultToolSelected && _clipboardService.HasCopiedObjects);

    #endregion

    #region CutCommand

    private ICommand? _cutCommand;

    public ICommand CutCommand => _cutCommand ??= new LambdaCommand(() =>
    {
        _clipboardService.Copy(SelectedObjects);
        _objects.RemoveAll(x => x.IsSelected);
        SelectedObjectsChanged();
    }, () => ToolsViewModel.IsDefaultToolSelected && HasSelectedObjects);

    #endregion

    #region DuplicateCommand

    private ICommand? _duplicateCommand;

    public ICommand DuplicateCommand => _duplicateCommand ??= new LambdaCommand(() =>
    {
        var objects = _clipboardService.Duplicate(SelectedObjects);
        _objects.AddRange(objects);
        StartFollowCursor(objects);
        UpdateCursorFollowingObjectsLocation();
    }, () => ToolsViewModel.IsDefaultToolSelected && HasSelectedObjects);

    #endregion

    #region AcceptCursorFollowingObjectsPositionCommand

    private ICommand? _acceptCursorFollowingObjectsPositionCommand;

    public ICommand AcceptCursorFollowingObjectsPositionCommand => _acceptCursorFollowingObjectsPositionCommand ??= new LambdaCommand(StopFollowCursor, () => HasCursorFollowingObjects);

    #endregion

    #region UpdateCursorFollowingObjectsLocationCommand

    private ICommand? _updateCursorFollowingObjectsLocationCommand;

    public ICommand UpdateCursorFollowingObjectsLocationCommand => _updateCursorFollowingObjectsLocationCommand ??= new LambdaCommand(UpdateCursorFollowingObjectsLocation, () => HasCursorFollowingObjects);

    #endregion

    #region RemoveCursorFollowingObjectsCommand

    private ICommand? _removeCursorFollowingObjectsCommand;

    public ICommand RemoveCursorFollowingObjectsCommand => _removeCursorFollowingObjectsCommand ??= new LambdaCommand(() =>
    {
        var cursorFollowingSet = _cursorFollowingObjects.Select(x => x.Object).ToHashSet();
        _objects.RemoveAll(cursorFollowingSet.Contains);
        StopFollowCursor();
    }, () => HasCursorFollowingObjects);

    #endregion

    #region RotateSelectedObjectsClockwiseCommand

    private ICommand? _rotateSelectedObjectsClockwiseCommand;

    public ICommand RotateSelectedObjectsClockwiseCommand => _rotateSelectedObjectsClockwiseCommand ??= new LambdaCommand(() =>
    {
        foreach (var obj in SelectedObjects)
        {
            obj.RotateClockwise();
        }
    }, () => ToolsViewModel.IsDefaultToolSelected && HasSelectedObjects);

    #endregion

    #region RotateSelectedObjectsCounterclockwiseCommand

    private ICommand? _rotateSelectedObjectsCounterclockwiseCommand;

    public ICommand RotateSelectedObjectsCounterclockwiseCommand => _rotateSelectedObjectsCounterclockwiseCommand ??= new LambdaCommand(() =>
    {
        foreach (var obj in SelectedObjects)
        {
            obj.RotateCounterclockwise();
        }
    }, () => ToolsViewModel.IsDefaultToolSelected && HasSelectedObjects);

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

        SetViewportCenterPoint(bounds.Select(x => x!.WorldBounds).GeometryUnion().Center);

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

    private void UpdateCursorFollowingObjectsLocation()
    {
        foreach (var (offset, obj) in _cursorFollowingObjects)
            obj.Location = (MousePosition - offset).ApplyGrid(GridStep);
    }

    private void StartFollowCursor(List<BaseObjectViewModel> objects)
    {
        var startPos = CalculateObjectsCenterPoint(objects);
        _cursorFollowingObjects = objects.Select(x => (startPos - x.Location, x)).ToList();
        _lastSelectedTool = _toolSwitcherService.CurrentTool;
        _toolSwitcherService.SwitchToEmptyTool();
        _toolSwitcherService.IsCurrentToolLocked = true;
        OnPropertyChanged(nameof(HasCursorFollowingObjects));
    }

    private void StopFollowCursor()
    {
        _cursorFollowingObjects.Clear();
        _toolSwitcherService.IsCurrentToolLocked = false;
        _toolSwitcherService.SwitchTool(_lastSelectedTool, false);
        _lastSelectedTool = null;
        OnPropertyChanged(nameof(HasCursorFollowingObjects));
    }

    private Vector2 CalculateObjectsCenterPoint(List<BaseObjectViewModel> objects)
    {
        var hitTestables = objects.Select(HitTester.GetFromContext).ToArray();

        if (hitTestables.All(x => x is { IsMeasured: true }))
            return hitTestables.Select(x => x!.WorldBounds).GeometryUnion().Center;

        return objects.Select(x => x.Location).Aggregate((a, b) => a + b) / objects.Count;
    }

    private void OnSimulationStateChanged(SimulationState oldState, SimulationState newState) =>
        _messageBus.Send(new SimulationStateChangedMessage(this, oldState, newState, _schemeSimulatorService.GetSimulationResult()));

    private void OnSelectedObjectsChanged()
    {
        var selectedObjects = SelectedObjects.ToList();

        _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.SelectedObjectsCount));

        if (selectedObjects.Count == 0)
        {
            _editorSelectionService.SetSchemeEditor(this);
            return;
        }

        _editorSelectionService.SetObjectsEditor(selectedObjects);

        OnPropertyChanged(nameof(SelectedObjects));
    }
}

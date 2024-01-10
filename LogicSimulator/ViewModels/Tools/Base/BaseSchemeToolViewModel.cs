using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using SharpDX;
using WpfExtensions.Mvvm;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.Tools.Base;

public class PlacingStep<T> where T : BaseObjectViewModel
{
    private readonly Action<T, Vector2> _stepAction;
    private readonly bool _useGrid;

    //TODO: Use grid
    public PlacingStep(Action<T, Vector2> stepAction, bool useGrid = true)
    {
        _stepAction = stepAction;
        _useGrid = useGrid;
    }

    public void Update(T obj, Vector2 pos) => _stepAction.Invoke(obj, pos);
}

public class RectanglePlacingToolViewModel : BasePlacingToolViewModel<RectangleViewModel>
{
    public RectanglePlacingToolViewModel(string name, SchemeViewModel scheme) : base(name, scheme)
    {
        //AddStep(pos =>
        //{
        //    Object!.Location = pos;
        //    Object.Width = 100;
        //    Object.Height = 100;
        //});

        AddStep((obj, pos) =>
        {
            obj.Width = pos.X - obj.Location.X;
            obj.Height = pos.Y - obj.Location.Y;
        });
    }
}

public abstract class BasePlacingToolViewModel<T> : BaseSchemeToolViewModel where T : BaseObjectViewModel, new()
{
    private int _stepIndex = -1;
    private readonly List<PlacingStep<T>> _steps = [];

    protected BasePlacingToolViewModel(string name, SchemeViewModel scheme) : base(name) => Scheme = scheme;

    public int StepsCount => _steps.Count;

    protected T? Object { get; private set; }

    protected SchemeViewModel Scheme { get; }

    #region StartPlacingCommand

    private ICommand? _startPlacingCommand;

    public ICommand StartPlacingCommand => _startPlacingCommand ??= new LambdaCommand<Vector2>(OnStartPlacing);

    #endregion

    #region NextPlacingCommand

    private ICommand? _nextPlacingCommand;

    public ICommand NextPlacingCommand => _nextPlacingCommand ??= new LambdaCommand<Vector2>(OnNextPlacingStep);

    #endregion

    #region UpdatePlacingCommand

    private ICommand? _updatePlacingCommand;

    public ICommand UpdatePlacingCommand => _updatePlacingCommand ??= new LambdaCommand<Vector2>(OnUpdatePlacing);

    #endregion

    #region EndPlacingCommand

    private ICommand? _endPlacingCommand;

    public ICommand EndPlacingCommand => _endPlacingCommand ??= new LambdaCommand(OnEndPlacing);

    #endregion

    #region CancelPlacingCommand

    private ICommand? _cancelPlacingCommand;

    public ICommand CancelPlacingCommand => _cancelPlacingCommand ??= new LambdaCommand(OnCancelPlacing);

    #endregion

    protected void AddStep(PlacingStep<T> step) => _steps.Add(step);

    protected void AddStep(Action<T, Vector2> stepAction, bool useGrid = true) => _steps.Add(new PlacingStep<T>(stepAction, useGrid));

    protected virtual void OnStartPlacing(Vector2 pos)
    {
        CanSwitch = false;
        foreach (var obj in Scheme.Objects)
            obj.IsSelected = false;
        Object = new T { IsSelected = true, Location = pos };
        Scheme.Objects.Add(Object);
        _stepIndex = 0;
    }

    protected virtual void OnNextPlacingStep(Vector2 pos)
    {
        _stepIndex++;
    }

    protected virtual void OnUpdatePlacing(Vector2 pos)
    {
        _steps[_stepIndex].Update(Object!, pos);
    }

    protected virtual void OnEndPlacing()
    {
        _stepIndex = -1;
        Object = null;
        CanSwitch = true;
    }

    protected virtual void OnCancelPlacing()
    {
        _stepIndex = -1;
        CanSwitch = true;
        //Object!.IsSelected = false;
        Scheme.Objects.Remove(Object!);
        Object = null;
    }
}

public abstract class BaseSchemeToolViewModel : BindableBase
{
    public event Action<BaseSchemeToolViewModel>? ToolSelected;

    protected BaseSchemeToolViewModel(string name) => _name = name;

    #region Name

    private string _name;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    #endregion

    #region IsActive

    private bool _isActive;

    public bool IsActive
    {
        get => _isActive;
        set => Set(ref _isActive, value);
    }

    #endregion

    #region CanSwitch

    private bool _canSwitch = true;

    public bool CanSwitch
    {
        get => _canSwitch;
        set => Set(ref _canSwitch, value);
    }

    #endregion

    #region SelectedCommand

    private ICommand? _selectedCommand;

    public ICommand SelectedCommand => _selectedCommand ??= new LambdaCommand(OnSelected);

    #endregion

    protected virtual void OnSelected() => ToolSelected?.Invoke(this);
}
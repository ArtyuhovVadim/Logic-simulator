using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using SharpDX;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.Tools.Base;

public class StepBuilder<T> where T : BaseObjectViewModel
{
    private readonly PlacingStepsBuilder<T> _parent;
    private readonly Action<T, Vector2> _stepAction;
    private bool _useGrid;

    public StepBuilder<T>? NextBuilder { get; set; }

    public StepBuilder(PlacingStepsBuilder<T> parent, Action<T, Vector2> stepAction)
    {
        _parent = parent;
        _stepAction = stepAction;
    }

    public StepBuilder<T> UseGrid()
    {
        _useGrid = true;
        return this;
    }

    public PlacingStepsBuilder<T> Then()
    {
        return _parent;
    }

    public PlacingStep<T> Build() => new(_stepAction) { UseGrid = _useGrid, NextStep = NextBuilder?.Build() };
}

public class PlacingStepsBuilder<T> where T : BaseObjectViewModel
{
    private StepBuilder<T>? _root;
    private StepBuilder<T>? _current;

    public StepBuilder<T> AddStep(Action<T, Vector2> stepAction)
    {
        if (_root is null)
        {
            _current = new StepBuilder<T>(this, stepAction);
            _root = _current;
            return _current;
        }

        var newStepBuilder = new StepBuilder<T>(this, stepAction);
        _current!.NextBuilder = newStepBuilder;
        _current = newStepBuilder;

        return _current;
    }

    public PlacingStep<T> Build() => _root!.Build();
}

public abstract class BasePlacingToolViewModel<T> : BaseSchemeToolViewModel where T : BaseObjectViewModel, new()
{
    private PlacingStep<T> _current = null!;

    private int _currentStep = -1;
    private List<PlacingStep<T>> _steps = [];

    protected BasePlacingToolViewModel(string name, SchemeViewModel scheme) : base(name)
    {
        Scheme = scheme;
        InitSteps();

        //TODO: Для теста
        while (_current is not null)
        {
            _steps.Add(_current);
            _current = _current.NextStep;
        }
    }

    private int StepsCount => _steps.Count;

    private bool IsStarted => _currentStep != -1;

    private bool InProgress => _currentStep >= 0 && _currentStep < StepsCount - 1;

    private bool IsLastStep => _currentStep == StepsCount - 1;

    protected T? Object { get; private set; }

    protected SchemeViewModel Scheme { get; }

    #region ActionCommand

    private ICommand? _actionCommand;

    public ICommand ActionCommand => _actionCommand ??= new LambdaCommand<Vector2>(pos =>
    {
        if (!IsStarted)
        {
            OnStartPlacing(pos);
            _currentStep++;
        }
        else if (InProgress)
        {
            _currentStep++;
        }
        else if (IsLastStep)
        {
            OnEndPlacing();
            _currentStep = -1;
        }
    });

    #endregion

    #region UpdateCommand

    private ICommand? _updateCommand;

    public ICommand UpdateCommand => _updateCommand ??= new LambdaCommand<Vector2>(pos =>
    {
        if (IsStarted)
        {
            OnUpdatePlacing(pos);
        }
    });

    #endregion

    #region RejectCommand

    private ICommand? _rejectCommand;

    public ICommand RejectCommand => _rejectCommand ??= new LambdaCommand(() =>
    {
        if (!IsStarted)
        {
            OnEndPlacing();
            Scheme.ToolsViewModel.CurrentTool = Scheme.ToolsViewModel.DefaultTool;
        }
        else
        {
            OnCancelPlacing();
            _currentStep = -1;
        }
    });

    #endregion

    protected abstract PlacingStep<T> ConfigurePlacingSteps(PlacingStepsBuilder<T> builder);

    protected void OnStartPlacing(Vector2 pos)
    {
        Scheme.ToolsViewModel.IsCurrentToolLocked = true;
        foreach (var obj in Scheme.Objects)
            obj.IsSelected = false;
        Object = new T { IsSelected = true };
        Scheme.Objects.Add(Object);
        Scheme.SelectedObjectsChanged();
    }

    protected void OnUpdatePlacing(Vector2 pos)
    {
        var step = _steps[_currentStep];
        step.Update(Object!, step.UseGrid ? pos.ApplyGrid(Scheme.GridStep) : pos);
    }

    protected void OnEndPlacing()
    {
        Object = null;
        Scheme.ToolsViewModel.IsCurrentToolLocked = false;
    }

    protected void OnCancelPlacing()
    {
        Scheme.ToolsViewModel.IsCurrentToolLocked = false;
        Scheme.Objects.Remove(Object!);
        Object = null;
        Scheme.SelectedObjectsChanged();
    }

    private void InitSteps() => _current = ConfigurePlacingSteps(new PlacingStepsBuilder<T>());
}
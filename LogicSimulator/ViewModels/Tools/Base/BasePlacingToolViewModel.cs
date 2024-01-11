using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using SharpDX;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.Tools.Base;

public abstract class BasePlacingToolViewModel<T> : BaseSchemeToolViewModel where T : BaseObjectViewModel, new()
{
    private int _currentStep = -1;
    private readonly List<PlacingStep<T>> _steps = [];

    protected BasePlacingToolViewModel(string name, SchemeViewModel scheme) : base(name) => Scheme = scheme;

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
            OnNextPlacingStep(pos);
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

    protected void OnStartPlacing(Vector2 pos)
    {
        Scheme.ToolsViewModel.IsCurrentToolLocked = true;
        foreach (var obj in Scheme.Objects)
            obj.IsSelected = false;
        Object = new T { IsSelected = true };
        Scheme.Objects.Add(Object);
        Scheme.SelectedObjectsChanged();
    }

    protected void OnNextPlacingStep(Vector2 pos)
    {
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

    protected void AddStep(PlacingStep<T> step) => _steps.Add(step);

    protected void AddStep(Action<T, Vector2> stepAction, bool useGrid = true) => _steps.Add(new PlacingStep<T>(stepAction, useGrid));

    /*protected virtual void OnStartPlacing(Vector2 pos)
    {
        //CanSwitch = false;
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
        //CanSwitch = true;
    }

    protected virtual void OnCancelPlacing()
    {
        _stepIndex = -1;
        //CanSwitch = true;
        //Object!.IsSelected = false;
        Scheme.Objects.Remove(Object!);
        Object = null;
    }*/
}
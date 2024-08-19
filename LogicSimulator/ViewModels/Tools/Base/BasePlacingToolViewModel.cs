using LogicSimulator.Models.Input;
using LogicSimulator.ViewModels.Anchorable;
using LogicSimulator.ViewModels.Objects.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools.Base;

public abstract class BasePlacingToolViewModel<T> : BaseToolViewModel where T : BaseObjectViewModel, new()
{
    private readonly Func<T> _objectFactory;
    private PlacingStep<T>? _currentStep;

    protected BasePlacingToolViewModel(SchemeViewModel scheme) : this(scheme, () => new T()) { }

    protected BasePlacingToolViewModel(SchemeViewModel scheme, Func<T> objectFactory)
    {
        _objectFactory = objectFactory;
        Scheme = scheme;
    }

    protected T? Object { get; private set; }

    protected SchemeViewModel Scheme { get; }

    protected abstract PlacingStep<T> FirstStep { get; }

    protected override void OnKeyDown(KeyInputArgs args)
    {
        if (args.Key != CanselKey) return;
        OnReject();
    }

    protected override void OnMouseLeftButtonDown(InputArgs args) => OnAction(args.Position);

    protected override void OnMouseMove(InputArgs args) => OnUpdate(args.Position);

    protected override void OnMouseRightButtonUp(InputArgs args) => OnReject();

    protected override void OnActivated()
    {
        _currentStep = FirstStep;
        Object = _objectFactory();
        Scheme.Objects.Add(Object);
        OnStartObjectPlacing(Object);
        _currentStep.EnterStep(Object, Scheme.MousePosition);
    }

    protected override void OnDeactivated()
    {
        if (Object is null) return;

        _currentStep!.ExitStep(Object, Scheme.MousePosition);
        Scheme.Objects.Remove(Object);
        Object = null;
    }

    protected virtual void OnStartObjectPlacing(T obj) { }

    protected virtual bool OnObjectPlaced(T obj) => true;

    protected void GoToStep(PlacingStep<T>? step) => GoToStepInternal(step, Scheme.MousePosition);

    protected void Reject() => OnReject();

    private void OnAction(Vector2 pos) => GoToStepInternal(_currentStep!.GetNextStep(Object!), pos);

    private void OnUpdate(Vector2 pos) => _currentStep?.Update(Object!, pos);

    private void OnReject()
    {
        Scheme.ToolsViewModel.IsCurrentToolLocked = false;

        if (_currentStep == FirstStep)
        {
            _currentStep!.ExitStep(Object!, Scheme.MousePosition);
            Scheme.Objects.Remove(Object!);
            Object = null;
            Scheme.ToolsViewModel.CurrentTool = Scheme.ToolsViewModel.DefaultTool;
        }
        else
        {
            Scheme.Objects.Remove(Object!);
            Object = _objectFactory();
            Scheme.Objects.Add(Object);
            _currentStep!.ExitStep(Object, Scheme.MousePosition);
            _currentStep = FirstStep;
            OnStartObjectPlacing(Object!);
            _currentStep.EnterStep(Object, Scheme.MousePosition);
        }
    }

    private void GoToStepInternal(PlacingStep<T>? step, Vector2 pos)
    {
        Scheme.ToolsViewModel.IsCurrentToolLocked = step != FirstStep;
        _currentStep!.ExitStep(Object!, pos);
        _currentStep = step;

        if (_currentStep is null)
        {
            if (!OnObjectPlaced(Object!)) Scheme.Objects.Remove(Object!);
            _currentStep = FirstStep;
            Object = _objectFactory();
            Scheme.Objects.Add(Object);
            Scheme.ToolsViewModel.IsCurrentToolLocked = false;
            OnStartObjectPlacing(Object!);
        }

        _currentStep.EnterStep(Object!, pos);
    }
}
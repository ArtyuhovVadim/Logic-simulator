using LogicSimulator.ViewModels.ObjectViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools.Base;

public delegate void EnterStep<in T>(T obj, Vector2 pos) where T : BaseObjectViewModel;

public delegate void UpdatePlacingStep<in T>(T obj, Vector2 pos) where T : BaseObjectViewModel;

public delegate void ExitStep<in T>(T obj, Vector2 pos) where T : BaseObjectViewModel;

public delegate PlacingStep<T>? PlacingStepTransition<T>(T obj) where T : BaseObjectViewModel;

public class PlacingStep<T> where T : BaseObjectViewModel
{
    private readonly EnterStep<T>? _enter;
    private readonly UpdatePlacingStep<T>? _update;
    private readonly ExitStep<T>? _exit;
    private readonly PlacingStepTransition<T> _transition;

    public PlacingStep(EnterStep<T>? enter, ExitStep<T>? exit, UpdatePlacingStep<T>? update, PlacingStepTransition<T> transition) : this(update, transition)
    {
        _enter = enter;
        _exit = exit;
    }

    public PlacingStep(UpdatePlacingStep<T>? update, PlacingStepTransition<T> transition)
    {
        _update = update;
        _transition = transition;
    }

    public void EnterStep(T obj, Vector2 pos) => _enter?.Invoke(obj, pos);

    public void Update(T obj, Vector2 pos) => _update?.Invoke(obj, pos);

    public void ExitStep(T obj, Vector2 pos) => _exit?.Invoke(obj, pos);

    public PlacingStep<T>? GetNextStep(T obj) => _transition(obj);
}
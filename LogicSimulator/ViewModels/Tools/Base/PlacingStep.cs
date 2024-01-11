using LogicSimulator.ViewModels.ObjectViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools.Base;

public class PlacingStep<T> where T : BaseObjectViewModel
{
    private readonly Action<T, Vector2> _stepAction;

    public bool UseGrid { get; }

    public PlacingStep(Action<T, Vector2> stepAction, bool useGrid = true)
    {
        UseGrid = useGrid;
        _stepAction = stepAction;
    }

    public void Update(T obj, Vector2 pos) => _stepAction.Invoke(obj, pos);
}
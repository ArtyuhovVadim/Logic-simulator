using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class PlacingStep
{
    private readonly SetPlacingValue _setPlacingValue;
    private readonly UpdatePlacingValueProgress _updatePlacingValueProgress;

    public bool UseGrid { get; private set; }

    public delegate void SetPlacingValue(Scene2D scene, Vector2 pos);

    public delegate void UpdatePlacingValueProgress(Scene2D scene, Vector2 pos);

    public PlacingStep(SetPlacingValue setPlacingValue, bool useGrid = true)
    {
        _setPlacingValue = setPlacingValue;
        UseGrid = useGrid;
    }

    public PlacingStep(SetPlacingValue setPlacingValue, UpdatePlacingValueProgress updatePlacingValueProgress, bool useGrid = true)
    {
        _setPlacingValue = setPlacingValue;
        _updatePlacingValueProgress = updatePlacingValueProgress;
        UseGrid = useGrid;
    }

    public void Start(Scene2D scene, Vector2 pos)
    {
        _setPlacingValue.Invoke(scene, pos);
    }

    public void Update(Scene2D scene, Vector2 pos)
    {
        _updatePlacingValueProgress?.Invoke(scene, pos);
    }
}
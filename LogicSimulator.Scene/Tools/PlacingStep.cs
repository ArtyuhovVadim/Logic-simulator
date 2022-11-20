using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class PlacingStep
{
    private readonly SetPlacingValue _setPlacingValue;
    private readonly UpdatePlacingValueProgress _updatePlacingValueProgress;

    public delegate void SetPlacingValue(Scene2D scene, Vector2 pos);

    public delegate void UpdatePlacingValueProgress(Scene2D scene, Vector2 pos);

    public PlacingStep(SetPlacingValue setPlacingValue)
    {
        _setPlacingValue = setPlacingValue;
    }

    public PlacingStep(SetPlacingValue setPlacingValue, UpdatePlacingValueProgress updatePlacingValueProgress)
    {
        _setPlacingValue = setPlacingValue;
        _updatePlacingValueProgress = updatePlacingValueProgress;
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
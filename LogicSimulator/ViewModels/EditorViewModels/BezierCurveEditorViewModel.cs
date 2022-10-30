using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class BezierCurveEditorViewModel : BaseEditorViewModel<BezierCurve>
{
    #region Point0

    public Vector2 Point0
    {
        get => Get<Vector2>();
        set => Set(value);
    }

    #endregion

    #region Point1

    public Vector2 Point1
    {
        get => Get<Vector2>();
        set => Set(value);
    }

    #endregion

    #region Point2

    public Vector2 Point2
    {
        get => Get<Vector2>();
        set => Set(value);
    }

    #endregion

    #region Point3

    public Vector2 Point3
    {
        get => Get<Vector2>();
        set => Set(value);
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => Get<float>();
        set => Set(value);
    }

    #endregion

    #region StrokeColor

    public Color4 StrokeColor
    {
        get => Get<Color4>();
        set => Set(value);
    }

    #endregion
}
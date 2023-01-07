using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class EllipseEditorViewModel : BaseEditorViewModel<Ellipse>
{
    #region Location

    public Vector2 Location
    {
        get => Get<Vector2>();
        set => Set(value);
    }

    #endregion

    #region RadiusX

    public float RadiusX
    {
        get => Get<float>();
        set => Set(value);
    }

    #endregion

    #region RadiusY

    public float RadiusY
    {
        get => Get<float>();
        set => Set(value);
    }

    #endregion

    #region FillColor

    public Color4 FillColor
    {
        get => Get<Color4>();
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

    #region StrokeThickness

    public float StrokeThickness
    {
        get => Get<float>();
        set => Set(value);
    }

    #endregion

    #region IsFilled

    public bool IsFilled
    {
        get => Get<bool>();
        set => Set(value);
    }

    #endregion
}
using LogicSimulator.ViewModels.EditorViewModels.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RectangleEditorViewModel : BaseEditorViewModel<Rectangle>
{
    #region Location

    public Vector2 Location
    {
        get => Get<Vector2>();
        set => Set(value);
    }

    #endregion

    #region Width

    public float Width
    {
        get => Get<float>();
        set => Set(value);
    }

    #endregion

    #region Height

    public float Height
    {
        get => Get<float>();
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

    #region FillColor

    public Color4 FillColor
    {
        get => Get<Color4>();
        set => Set(value);
    }

    #endregion
}
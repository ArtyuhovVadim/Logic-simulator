using System.Linq;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels;

public class RectangleEditorViewModel : BaseEditorViewModel
{
    #region Location

    public Vector2 Location
    {
        get => ((Rectangle)SceneObjects.First()).Location;
        set => ((Rectangle)SceneObjects.First()).Location = value;
    }

    #endregion

    #region LocationX

    public float Width
    {
        get => ((Rectangle)SceneObjects.First()).Width;
        set => ((Rectangle)SceneObjects.First()).Width = value;
    }

    #endregion

    #region LocationY

    public float Height
    {
        get => ((Rectangle)SceneObjects.First()).Height;
        set => ((Rectangle)SceneObjects.First()).Height = value;
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => ((Rectangle)SceneObjects.First()).StrokeThickness;
        set => ((Rectangle)SceneObjects.First()).StrokeThickness = value;
    }

    #endregion
}
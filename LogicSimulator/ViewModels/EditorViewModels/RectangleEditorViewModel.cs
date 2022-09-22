using System.Linq;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RectangleEditorViewModel : BaseEditorViewModel<Rectangle>
{
    #region Location

    public Vector2 Location
    {
        get => Objects.First().Location;
        set => Objects.First().Location = value;
    }

    #endregion

    #region LocationX

    public float Width
    {
        get => Objects.First().Width;
        set => Objects.First().Width = value;
    }

    #endregion

    #region LocationY

    public float Height
    {
        get => Objects.First().Height;
        set => Objects.First().Height = value;
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => Objects.First().StrokeThickness;
        set => Objects.First().StrokeThickness = value;
    }

    #endregion

}
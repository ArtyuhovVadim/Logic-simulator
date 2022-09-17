using System.Linq;
using LogicSimulator.Scene.SceneObjects;

namespace LogicSimulator.ViewModels;

public class RectangleEditorViewModel : BaseEditorViewModel
{
    #region LocationX

    public float LocationX
    {
        get => ((Rectangle)SceneObjects.First()).Location.X;
        set => ((Rectangle)SceneObjects.First()).Location = ((Rectangle)SceneObjects.First()).Location with { X = value };
    }

    #endregion

    #region LocationY

    public float LocationY
    {
        get => ((Rectangle)SceneObjects.First()).Location.Y;
        set => ((Rectangle)SceneObjects.First()).Location = ((Rectangle)SceneObjects.First()).Location with { Y = value };
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
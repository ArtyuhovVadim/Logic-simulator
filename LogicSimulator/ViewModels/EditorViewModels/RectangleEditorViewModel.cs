using LogicSimulator.ViewModels.EditorViewModels.Base;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RectangleEditorViewModel : BaseEditorViewModel<Rectangle>
{
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
}
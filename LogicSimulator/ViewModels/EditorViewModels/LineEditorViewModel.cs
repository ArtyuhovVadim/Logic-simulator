using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class LineEditorViewModel : BaseEditorViewModel<Line>
{
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
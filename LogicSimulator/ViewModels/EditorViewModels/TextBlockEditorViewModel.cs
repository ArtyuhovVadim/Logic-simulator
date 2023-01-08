using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

//TODO: !!!
public class TextBlockEditorViewModel : BaseEditorViewModel<TextBlock>
{
    protected override EditorLayout CreateLayout() => new("Текст", new[]
    {
        new EditorGroup("Расположение", new []
        {
            new EditorRow(CreateObjectPropertyViewModel<Vector2>(nameof(TextBlock.Location))),
        })
    });

    #region Location

    public Vector2 Location
    {
        get => Get<Vector2>();
        set => Set(value);
    }

    #endregion

    #region Text

    public string Text
    {
        get => Get<string>();
        set => Set(value);
    }

    #endregion

    #region FontName

    public string FontName
    {
        get => Get<string>();
        set => Set(value);
    }

    #endregion

    #region FontSize

    public float FontSize
    {
        get => Get<float>();
        set => Set(value);
    }

    #endregion

    #region IsBold

    public bool IsBold
    {
        get => Get<bool>();
        set => Set(value);
    }

    #endregion

    #region IsItalic

    public bool IsItalic
    {
        get => Get<bool>();
        set => Set(value);
    }

    #endregion

    #region IsUnderlined

    public bool IsUnderlined
    {
        get => Get<bool>();
        set => Set(value);
    }

    #endregion

    #region IsCross

    public bool IsCross
    {
        get => Get<bool>();
        set => Set(value);
    }

    #endregion

    #region TextColor

    public Color4 TextColor
    {
        get => Get<Color4>();
        set => Set(value);
    }

    #endregion
}
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RectangleEditorViewModel : BaseEditorViewModel<Rectangle>
{
    public Vector2 Location { get => Get<Vector2>(); set => Set(value); }
    public float Width { get => Get<float>(); set => Set(value); }
    public float Height { get => Get<float>(); set => Set(value); }
    public float StrokeThickness { get => Get<float>(); set => Set(value); }
    public Color4 StrokeColor { get => Get<Color4>(); set => Set(value); }
    public bool IsFilled { get => Get<bool>(); set => Set(value); }
    public Color4 FillColor { get => Get<Color4>(); set => Set(value); }

    protected override EditorLayout CreateLayout() => new("Прямоугольник", new[]
    {
        new EditorGroup("Расположение", new []
        {
            new EditorRow(CreateObjectPropertyViewModel<Vector2>(nameof(Rectangle.Location))),
        }),
        new EditorGroup("Свойства", new []
        {
            new EditorRow("Ширина", CreateObjectPropertyViewModel<float>(nameof(Rectangle.Width))),
            new EditorRow("Высота", CreateObjectPropertyViewModel<float>(nameof(Rectangle.Height))),
            new EditorRow("Граница", new[]
            {
                CreateObjectPropertyViewModel<float>(nameof(Rectangle.StrokeThickness)),
                CreateObjectPropertyViewModel<Color4>(nameof(Rectangle.StrokeColor))
            }),
            new EditorRow("Цвет заливки", new[]
            {
                CreateObjectPropertyViewModel<Color4>(nameof(Rectangle.FillColor)),
                CreateObjectPropertyViewModel<bool>(nameof(Rectangle.IsFilled))
            })
        })
    });
}
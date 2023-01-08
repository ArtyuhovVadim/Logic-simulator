using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RoundedRectangleEditorViewModel : BaseEditorViewModel<RoundedRectangle> 
{
    public Vector2 Location { get => Get<Vector2>(); set => Set(value); }
    public float Width { get => Get<float>(); set => Set(value); }
    public float Height { get => Get<float>(); set => Set(value); }
    public float RadiusX { get => Get<float>(); set => Set(value); }
    public float RadiusY { get => Get<float>(); set => Set(value); }
    public float StrokeThickness { get => Get<float>(); set => Set(value); }
    public Color4 StrokeColor { get => Get<Color4>(); set => Set(value); }
    public bool IsFilled { get => Get<bool>(); set => Set(value); }
    public Color4 FillColor { get => Get<Color4>(); set => Set(value); }

    protected override EditorLayout CreateLayout() => new("Закругленный прямоугольник", new[]
    {
        new EditorGroup("Расположение", new []
        {
            new EditorRow(CreateObjectPropertyViewModel<Vector2>(nameof(RoundedRectangle.Location))),
        }),
        new EditorGroup("Свойства", new []
        {
            new EditorRow("Ширина", CreateObjectPropertyViewModel<float>(nameof(RoundedRectangle.Width))),
            new EditorRow("Высота", CreateObjectPropertyViewModel<float>(nameof(RoundedRectangle.Height))),
            new EditorRow("Радиус X", CreateObjectPropertyViewModel<float>(nameof(RoundedRectangle.RadiusX))),
            new EditorRow("Радиус Y", CreateObjectPropertyViewModel<float>(nameof(RoundedRectangle.RadiusY))),
            new EditorRow("Граница", new[]
            {
                CreateObjectPropertyViewModel<float>(nameof(RoundedRectangle.StrokeThickness)),
                CreateObjectPropertyViewModel<Color4>(nameof(RoundedRectangle.StrokeColor))
            }),
            new EditorRow("Цвет заливки", new[]
            {
                CreateObjectPropertyViewModel<Color4>(nameof(RoundedRectangle.FillColor)),
                CreateObjectPropertyViewModel<bool>(nameof(RoundedRectangle.IsFilled))
            })
        })
    });
}
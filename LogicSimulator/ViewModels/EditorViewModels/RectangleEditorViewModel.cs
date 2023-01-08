using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RectangleEditorViewModel : BaseEditorViewModel<Rectangle>
{
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
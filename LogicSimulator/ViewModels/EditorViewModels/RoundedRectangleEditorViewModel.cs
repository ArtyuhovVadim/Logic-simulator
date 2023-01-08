using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RoundedRectangleEditorViewModel : BaseEditorViewModel<RoundedRectangle> 
{
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
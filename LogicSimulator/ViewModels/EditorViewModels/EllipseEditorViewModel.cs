using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class EllipseEditorViewModel : BaseEditorViewModel<Ellipse>
{
    protected override EditorLayout CreateLayout() => new("Эллипс", new[]
    {
        new EditorGroup("Расположение", new []
        {
            new EditorRow(CreateObjectPropertyViewModel<Vector2>(nameof(Ellipse.Location))),
        }),
        new EditorGroup("Свойства", new []
        {
            new EditorRow("Радиус X", CreateObjectPropertyViewModel<float>(nameof(Ellipse.RadiusX))),
            new EditorRow("Радиус Y", CreateObjectPropertyViewModel<float>(nameof(Ellipse.RadiusY))),
            new EditorRow("Граница", new[]
            {
                CreateObjectPropertyViewModel<float>(nameof(Ellipse.StrokeThickness)),
                CreateObjectPropertyViewModel<Color4>(nameof(Ellipse.StrokeColor))
            }),
            new EditorRow("Цвет заливки", new[]
            {
                CreateObjectPropertyViewModel<Color4>(nameof(Ellipse.FillColor)),
                CreateObjectPropertyViewModel<bool>(nameof(Ellipse.IsFilled))
            })
        })
    });
}
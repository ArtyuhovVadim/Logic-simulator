using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class BezierCurveEditorViewModel : BaseEditorViewModel<BezierCurve>
{
    protected override EditorLayout CreateLayout() => new("Кривая Безье", new[]
    {
        new EditorGroup("Расположение", new []
        {
            new EditorRow(CreateObjectPropertyViewModel<Vector2>(nameof(BezierCurve.Location))),
        }),
        new EditorGroup("Вершины (TODO: Название)", new []
        {
            new EditorRow(CreateObjectPropertyViewModel<Vector2>(nameof(BezierCurve.Point1))),
            new EditorRow(CreateObjectPropertyViewModel<Vector2>(nameof(BezierCurve.Point2))),
            new EditorRow(CreateObjectPropertyViewModel<Vector2>(nameof(BezierCurve.Point3))),
        }),
        new EditorGroup("Свойства", new []
        {
            new EditorRow("Граница", new[]
            {
                CreateObjectPropertyViewModel<float>(nameof(BezierCurve.StrokeThickness)),
                CreateObjectPropertyViewModel<Color4>(nameof(BezierCurve.StrokeColor))
            })
        })
    });
}
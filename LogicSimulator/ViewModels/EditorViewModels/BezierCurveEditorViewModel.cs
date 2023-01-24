using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class BezierCurveEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Кривая Безье")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(Rectangle.Location)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Поворот")
                .WithSingleProperty<RotationEnumPropertyViewModel>(nameof(Rectangle.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вершины")
            .WithRow(rowBuilder => rowBuilder
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(BezierCurve.Point1)))
            .WithRow(rowBuilder => rowBuilder
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(BezierCurve.Point2)))
            .WithRow(rowBuilder => rowBuilder
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(BezierCurve.Point3))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(BezierCurve.StrokeThickness))
                .WithSingleProperty<Color4PropertyViewModel>(nameof(BezierCurve.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize())))
        .Build();
}
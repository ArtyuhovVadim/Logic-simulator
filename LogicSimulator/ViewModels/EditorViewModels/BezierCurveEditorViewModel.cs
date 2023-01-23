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
                .WithProperty<Vector2PropertyViewModel>(nameof(BezierCurve.Location))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вершины")
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<Vector2PropertyViewModel>(nameof(BezierCurve.Point1)))
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<Vector2PropertyViewModel>(nameof(BezierCurve.Point2)))
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<Vector2PropertyViewModel>(nameof(BezierCurve.Point3))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithProperty<FloatPropertyViewModel>(nameof(BezierCurve.StrokeThickness))
                .WithProperty<Color4PropertyViewModel>(nameof(BezierCurve.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize())))
        .Build();
}
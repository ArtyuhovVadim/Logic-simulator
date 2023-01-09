using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class BezierCurveEditorViewModel : BaseEditorViewModel<BezierCurve>
{
    public Vector2 Location { get => Get<Vector2>(); set => Set(value); }
    public Vector2 Point1 { get => Get<Vector2>(); set => Set(value); }
    public Vector2 Point2 { get => Get<Vector2>(); set => Set(value); }
    public Vector2 Point3 { get => Get<Vector2>(); set => Set(value); }
    public float StrokeThickness { get => Get<float>(); set => Set(value); }
    public Color4 StrokeColor { get => Get<Color4>(); set => Set(value); }

    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create()
        .WithName("Кривая Безье")
        .WithGroup(groupBuilder => groupBuilder
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<Vector2>(nameof(BezierCurve.Location))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вершины (TODO: Название)")
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<Vector2>(nameof(BezierCurve.Point1))
                .WithProperty<Vector2>(nameof(BezierCurve.Point2))
                .WithProperty<Vector2>(nameof(BezierCurve.Point3))))
        .WithGroup(groupBuilder => groupBuilder
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithProperty<float>(nameof(BezierCurve.StrokeThickness))
                .WithProperty<Color4>(nameof(BezierCurve.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize())))
        .Build();
}
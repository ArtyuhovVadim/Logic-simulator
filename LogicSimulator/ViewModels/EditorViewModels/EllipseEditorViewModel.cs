using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class EllipseEditorViewModel : BaseEditorViewModel<Ellipse>
{
    public Vector2 Location { get => Get<Vector2>(); set => Set(value); }
    public float RadiusX { get => Get<float>(); set => Set(value); }
    public float RadiusY { get => Get<float>(); set => Set(value); }
    public float StrokeThickness { get => Get<float>(); set => Set(value); }
    public Color4 StrokeColor { get => Get<Color4>(); set => Set(value); }
    public bool IsFilled { get => Get<bool>(); set => Set(value); }
    public Color4 FillColor { get => Get<Color4>(); set => Set(value); }

    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create()
        .WithName("Эллипс")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<Vector2>(nameof(Ellipse.Location))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус X")
                .WithProperty<float>(nameof(Ellipse.RadiusX)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус Y")
                .WithProperty<float>(nameof(Ellipse.RadiusY)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithProperty<float>(nameof(Ellipse.StrokeThickness))
                .WithProperty<Color4>(nameof(Ellipse.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize()))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithProperty<Color4>(nameof(Ellipse.FillColor))
                .WithProperty<bool>(nameof(Ellipse.IsFilled))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithAutoSize()
                    .WithAutoSize())))
        .Build();
}
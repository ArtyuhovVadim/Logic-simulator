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

    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create()
        .WithName("Закругленный прямоугольник")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<Vector2>(nameof(RoundedRectangle.Location))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Ширина")
                .WithProperty<float>(nameof(RoundedRectangle.Width)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Высота")
                .WithProperty<float>(nameof(RoundedRectangle.Height)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус X")
                .WithProperty<float>(nameof(RoundedRectangle.RadiusX)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус Y")
                .WithProperty<float>(nameof(RoundedRectangle.RadiusY)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithProperty<float>(nameof(RoundedRectangle.StrokeThickness))
                .WithProperty<Color4>(nameof(RoundedRectangle.StrokeColor)))
        .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithProperty<Color4>(nameof(RoundedRectangle.FillColor))
                .WithProperty<bool>(nameof(RoundedRectangle.IsFilled))))
        .Build();
}
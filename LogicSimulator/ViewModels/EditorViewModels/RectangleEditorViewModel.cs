using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RectangleEditorViewModel : BaseEditorViewModel<Rectangle>
{
    public Vector2 Location { get => Get<Vector2>(); set => Set(value); }
    public float Width { get => Get<float>(); set => Set(value); }
    public float Height { get => Get<float>(); set => Set(value); }
    public float StrokeThickness { get => Get<float>(); set => Set(value); }
    public Color4 StrokeColor { get => Get<Color4>(); set => Set(value); }
    public bool IsFilled { get => Get<bool>(); set => Set(value); }
    public Color4 FillColor { get => Get<Color4>(); set => Set(value); }

    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create()
        .WithName("Прямоугольник")
        // .WithGroup(groupBuilder => groupBuilder
        //     .WithGroupName("Расположение")
        //     .WithRow(rowBuilder => rowBuilder
        //         .WithProperty<Vector2>(nameof(Rectangle.Location))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Ширина")
                .WithConcreteProperty(nameof(Rectangle.Width), name => new FloatPropertyViewModel(name, () => Objects)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Высота")
                .WithConcreteProperty(nameof(Rectangle.Height), name => new FloatPropertyViewModel(name, () => Objects))))
        /*.WithRow(rowBuilder => rowBuilder
            .WithRowName("Граница")
            .WithProperty<float>(nameof(Rectangle.StrokeThickness))
            .WithProperty<Color4>(nameof(Rectangle.StrokeColor))
            .WithLayout(layoutBuilder => layoutBuilder
                .WithRelativeSize(1)
                .WithAutoSize()))
        .WithRow(rowBuilder => rowBuilder
            .WithRowName("Цвет заливки")
            .WithProperty<Color4>(nameof(Rectangle.FillColor))
            .WithProperty<bool>(nameof(Rectangle.IsFilled))
            .WithLayout(layoutBuilder => layoutBuilder
                .WithAutoSize()
                .WithAutoSize())))*/
        .Build();
}
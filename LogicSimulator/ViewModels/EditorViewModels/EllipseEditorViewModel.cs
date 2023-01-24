using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class EllipseEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Эллипс")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithProperty<Vector2PropertyViewModel>(nameof(Rectangle.Location)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Поворот")
                .WithProperty<RotationEnumPropertyViewModel>(nameof(Rectangle.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус X")
                .WithProperty<FloatPropertyViewModel>(nameof(Ellipse.RadiusX)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус Y")
                .WithProperty<FloatPropertyViewModel>(nameof(Ellipse.RadiusY)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithProperty<FloatPropertyViewModel>(nameof(Ellipse.StrokeThickness))
                .WithProperty<Color4PropertyViewModel>(nameof(Ellipse.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize()))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithProperty<Color4PropertyViewModel>(nameof(Ellipse.FillColor))
                .WithProperty<BoolPropertyViewModel>(nameof(Ellipse.IsFilled))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithAutoSize()
                    .WithAutoSize())))
        .Build();
}
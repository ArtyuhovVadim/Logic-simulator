using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base.Properties;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using LogicSimulator.ViewModels.ObjectViewModels;

namespace LogicSimulator.ViewModels.EditorViewModels;

[Editor(typeof(EllipseViewModel))]
public class EllipseEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Эллипс")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(EllipseViewModel.Location)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Поворот")
                .WithSingleProperty<RotationEnumPropertyViewModel>(nameof(EllipseViewModel.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус X")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(EllipseViewModel.RadiusX)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус Y")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(EllipseViewModel.RadiusY)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(EllipseViewModel.StrokeThickness))
                .WithSingleProperty<ColorPropertyViewModel>(nameof(EllipseViewModel.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize()))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithSingleProperty<ColorPropertyViewModel>(nameof(EllipseViewModel.FillColor))
                .WithSingleProperty<BoolPropertyViewModel>(nameof(EllipseViewModel.IsFilled))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithAutoSize()
                    .WithAutoSize())))
        .Build();
}
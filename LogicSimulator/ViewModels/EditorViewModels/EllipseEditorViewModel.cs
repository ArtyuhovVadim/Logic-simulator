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
                .WithSingleProperty<EnumPropertyViewModel>(nameof(EllipseViewModel.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус X")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(EllipseViewModel.RadiusX), OneOrMoreConfigure))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус Y")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(EllipseViewModel.RadiusY), OneOrMoreConfigure))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithMultiProperty<StrokePropertiesViewModel>(multiPropertyBuilder => multiPropertyBuilder
                    .WithProperty<EnumPropertyViewModel>(nameof(EllipseViewModel.StrokeThicknessType))
                    .WithProperty<FloatPropertyViewModel>(nameof(EllipseViewModel.StrokeThickness), OneOrMoreConfigure)
                    .WithProperty<ColorPropertyViewModel>(nameof(EllipseViewModel.StrokeColor))))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithSingleProperty<ColorPropertyViewModel>(nameof(EllipseViewModel.FillColor))
                .WithSingleProperty<BoolPropertyViewModel>(nameof(EllipseViewModel.IsFilled))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithAutoSize()
                    .WithAutoSize())))
        .Build();
}
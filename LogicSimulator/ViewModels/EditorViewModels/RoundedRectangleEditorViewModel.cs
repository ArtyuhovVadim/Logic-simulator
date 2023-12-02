using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base.Properties;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using LogicSimulator.ViewModels.ObjectViewModels;

namespace LogicSimulator.ViewModels.EditorViewModels;

[Editor(typeof(RoundedRectangleViewModel))]
public class RoundedRectangleEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Закругленный прямоугольник")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(RoundedRectangleViewModel.Location)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Поворот")
                .WithSingleProperty<RotationEnumPropertyViewModel>(nameof(RoundedRectangleViewModel.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Ширина")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(RoundedRectangleViewModel.Width)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Высота")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(RoundedRectangleViewModel.Height)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус X")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(RoundedRectangleViewModel.RadiusX)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус Y")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(RoundedRectangleViewModel.RadiusY)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(RoundedRectangleViewModel.StrokeThickness))
                .WithSingleProperty<ColorPropertyViewModel>(nameof(RoundedRectangleViewModel.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize()))
        .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
               .WithSingleProperty<ColorPropertyViewModel>(nameof(RoundedRectangleViewModel.FillColor))
               .WithSingleProperty<BoolPropertyViewModel>(nameof(RoundedRectangleViewModel.IsFilled))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithAutoSize()
                    .WithAutoSize())))
        .Build();
}
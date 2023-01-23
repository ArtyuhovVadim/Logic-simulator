using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RoundedRectangleEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Закругленный прямоугольник")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<Vector2PropertyViewModel>(nameof(RoundedRectangle.Location))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Ширина")
                .WithProperty<FloatPropertyViewModel>(nameof(RoundedRectangle.Width)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Высота")
                .WithProperty<FloatPropertyViewModel>(nameof(RoundedRectangle.Height)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус X")
                .WithProperty<FloatPropertyViewModel>(nameof(RoundedRectangle.RadiusX)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Радиус Y")
                .WithProperty<FloatPropertyViewModel>(nameof(RoundedRectangle.RadiusY)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithProperty<FloatPropertyViewModel>(nameof(RoundedRectangle.StrokeThickness))
                .WithProperty<Color4PropertyViewModel>(nameof(RoundedRectangle.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize()))
        .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithProperty<Color4PropertyViewModel>(nameof(RoundedRectangle.FillColor))
                .WithProperty<BoolPropertyViewModel>(nameof(RoundedRectangle.IsFilled))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithAutoSize()
                    .WithAutoSize())))
        .Build();
}
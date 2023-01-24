using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RectangleEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Прямоугольник")
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
                .WithRowName("Ширина")
                .WithProperty<FloatPropertyViewModel>(nameof(Rectangle.Width)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Высота")
                .WithProperty<FloatPropertyViewModel>(nameof(Rectangle.Height)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithProperty<FloatPropertyViewModel>(nameof(Rectangle.StrokeThickness))
                .WithProperty<Color4PropertyViewModel>(nameof(Rectangle.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize()))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithProperty<Color4PropertyViewModel>(nameof(Rectangle.FillColor))
                .WithProperty<BoolPropertyViewModel>(nameof(Rectangle.IsFilled))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithAutoSize()
                    .WithAutoSize())))
        .Build();
}
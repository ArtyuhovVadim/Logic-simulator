using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base.Properties;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using LogicSimulator.ViewModels.ObjectViewModels.Gates;

namespace LogicSimulator.ViewModels.EditorViewModels;

[Editor(typeof(InputGateViewModel))]
public class InputGateEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Вход")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(InputGateViewModel.Location), ConfigureAsPositionVector))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Поворот")
                .WithSingleProperty<EnumPropertyViewModel>(nameof(InputGateViewModel.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Масштаб")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(InputGateViewModel.Scale)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithMultiProperty<StrokePropertiesViewModel>(multiPropertyBuilder => multiPropertyBuilder
                    .WithProperty<EnumPropertyViewModel>(nameof(InputGateViewModel.StrokeThicknessType))
                    .WithProperty<FloatPropertyViewModel>(nameof(InputGateViewModel.StrokeThickness), ConfigureAsSizeNumber)
                    .WithProperty<ColorPropertyViewModel>(nameof(InputGateViewModel.StrokeColor))))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithSingleProperty<ColorPropertyViewModel>(nameof(InputGateViewModel.FillColor))))
        .Build();
}
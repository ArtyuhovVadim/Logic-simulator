using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base.Properties;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using LogicSimulator.ViewModels.ObjectViewModels.Gates;

namespace LogicSimulator.ViewModels.EditorViewModels;

[Editor(typeof(AndGateViewModel))]
public class AndGateEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Логический вентиль И")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(AndGateViewModel.Location), ConfigureAsPositionVector))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Поворот")
                .WithSingleProperty<EnumPropertyViewModel>(nameof(AndGateViewModel.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithMultiProperty<StrokePropertiesViewModel>(multiPropertyBuilder => multiPropertyBuilder
                    .WithProperty<EnumPropertyViewModel>(nameof(AndGateViewModel.StrokeThicknessType))
                    .WithProperty<FloatPropertyViewModel>(nameof(AndGateViewModel.StrokeThickness), ConfigureAsSizeNumber)
                    .WithProperty<ColorPropertyViewModel>(nameof(AndGateViewModel.StrokeColor))))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithSingleProperty<ColorPropertyViewModel>(nameof(AndGateViewModel.FillColor))))
        .Build();
}
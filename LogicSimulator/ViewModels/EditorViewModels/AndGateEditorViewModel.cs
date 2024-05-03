using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.ExtensionMethods;
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
        .WithLocationRotationGroup()
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Задержка")
                .WithSingleProperty<NumberPropertyViewModel<ulong>>(nameof(AndGateViewModel.Delay))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вид")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Масштаб")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(AndGateViewModel.Scale)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Зазор портов")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(AndGateViewModel.InputPortsSpacing)))
            .WithBorderRow()
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithSingleProperty<ColorPropertyViewModel>(nameof(AndGateViewModel.FillColor))))
        .Build();
}
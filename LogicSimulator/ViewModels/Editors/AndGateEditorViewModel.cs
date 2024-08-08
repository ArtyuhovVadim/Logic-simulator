using LogicSimulator.Infrastructure.Attributes;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;
using LogicSimulator.ViewModels.Logic.Gates;

namespace LogicSimulator.ViewModels.Editors;

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
                .WithRowName("Имя")
                .WithSingleProperty<StringPropertyViewModel>(nameof(AndGateViewModel.Name)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Задержка")
                .WithSingleProperty<NumberPropertyViewModel<ulong>>(nameof(AndGateViewModel.Delay)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Кол-во входов")
                //TODO: Ограничить максимальное число
                .WithSingleProperty<NumberPropertyViewModel<int>>(nameof(AndGateViewModel.InputPortsCount), prop => prop.MinNumber = 2)))
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
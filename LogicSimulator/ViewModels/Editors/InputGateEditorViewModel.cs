using LogicSimulator.Infrastructure.Attributes;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;
using LogicSimulator.ViewModels.Logic.Gates;

namespace LogicSimulator.ViewModels.Editors;

[Editor(typeof(InputGateViewModel))]
public class InputGateEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Вход")
        .WithLocationRotationGroup()
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Имя")
                .WithSingleProperty<StringPropertyViewModel>(nameof(InputGateViewModel.Name)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Задержка")
                .WithSingleProperty<NumberPropertyViewModel<ulong>>(nameof(InputGateViewModel.Delay))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вид")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Масштаб")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(InputGateViewModel.Scale)))
            .WithBorderRow()
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithSingleProperty<ColorPropertyViewModel>(nameof(InputGateViewModel.FillColor))))
        .Build();
}
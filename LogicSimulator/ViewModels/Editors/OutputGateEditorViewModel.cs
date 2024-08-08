using LogicSimulator.Infrastructure.Attributes;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;
using LogicSimulator.ViewModels.Logic.Gates;

namespace LogicSimulator.ViewModels.Editors;

[Editor(typeof(OutputGateViewModel))]
public class OutputGateEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Выход")
        .WithLocationRotationGroup()
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Имя")
                .WithSingleProperty<StringPropertyViewModel>(nameof(OutputGateViewModel.Name))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вид")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Масштаб")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(OutputGateViewModel.Scale)))
            .WithBorderRow()
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithSingleProperty<ColorPropertyViewModel>(nameof(OutputGateViewModel.FillColor))))
        .Build();
}
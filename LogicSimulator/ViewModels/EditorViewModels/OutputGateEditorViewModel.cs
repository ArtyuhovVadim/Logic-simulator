using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base.Properties;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using LogicSimulator.ViewModels.ObjectViewModels.Gates;

namespace LogicSimulator.ViewModels.EditorViewModels;

[Editor(typeof(OutputGateViewModel))]
public class OutputGateEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Выход")
        .WithLocationRotationGroup()
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
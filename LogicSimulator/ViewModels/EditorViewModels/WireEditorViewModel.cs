using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base.Properties;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using LogicSimulator.ViewModels.ObjectViewModels;

namespace LogicSimulator.ViewModels.EditorViewModels;

[Editor(typeof(WireViewModel))]
public class WireEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Провод")
        .WithLocationRotationGroup()
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вершины")
            .WithRow(rowBuilder => rowBuilder
                .WithSingleProperty<VerticesPropertyViewModel>(nameof(WireViewModel.Vertexes))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вид")
            .WithBorderRow())
        .Build();
}
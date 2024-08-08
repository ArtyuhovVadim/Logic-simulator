using LogicSimulator.Infrastructure.Attributes;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;
using LogicSimulator.ViewModels.Logic;

namespace LogicSimulator.ViewModels.Editors;

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
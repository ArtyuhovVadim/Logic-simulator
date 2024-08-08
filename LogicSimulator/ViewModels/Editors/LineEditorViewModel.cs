using LogicSimulator.Infrastructure.Attributes;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;
using LogicSimulator.ViewModels.Objects;

namespace LogicSimulator.ViewModels.Editors;

[Editor(typeof(LineViewModel))]
public class LineEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Ломаная линия")
        .WithLocationRotationGroup()
         .WithGroup(groupBuilder => groupBuilder
             .WithGroupName("Вершины")
             .WithRow(rowBuilder => rowBuilder
                 .WithSingleProperty<VerticesPropertyViewModel>(nameof(LineViewModel.Vertexes))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithBorderRow())
        .Build();
}
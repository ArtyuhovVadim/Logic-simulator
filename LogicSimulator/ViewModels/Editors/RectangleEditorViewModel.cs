using LogicSimulator.Infrastructure.Attributes;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Objects;

namespace LogicSimulator.ViewModels.Editors;

[Editor(typeof(RectangleViewModel))]
public class RectangleEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Прямоугольник")
        .WithLocationRotationGroup()
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithSizeRow()
            .WithBorderRow()
            .WithFillRow())
        .Build();
}
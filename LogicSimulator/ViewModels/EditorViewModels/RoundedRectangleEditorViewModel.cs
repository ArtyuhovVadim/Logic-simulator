using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using LogicSimulator.ViewModels.ObjectViewModels;

namespace LogicSimulator.ViewModels.EditorViewModels;

[Editor(typeof(RoundedRectangleViewModel))]
public class RoundedRectangleEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Закругленный прямоугольник")
        .WithLocationRotationGroup()
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithSizeRow()
            .WithRadiusRow()
            .WithBorderRow()
            .WithFillRow())
        .Build();
}
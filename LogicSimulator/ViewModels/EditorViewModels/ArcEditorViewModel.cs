using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base.Properties;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using LogicSimulator.ViewModels.ObjectViewModels;

namespace LogicSimulator.ViewModels.EditorViewModels;

[Editor(typeof(ArcViewModel))]
public class ArcEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Дуга")
        .WithLocationRotationGroup()
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Начальный угол")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(ArcViewModel.StartAngle), ConfigureAsAngleNumber))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Конечный угол")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(ArcViewModel.EndAngle), ConfigureAsAngleNumber))
            .WithRadiusRow()
            .WithBorderRow())
        .Build();
}
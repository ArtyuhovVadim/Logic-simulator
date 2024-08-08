using LogicSimulator.Infrastructure.Attributes;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;
using LogicSimulator.ViewModels.Objects;

namespace LogicSimulator.ViewModels.Editors;

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
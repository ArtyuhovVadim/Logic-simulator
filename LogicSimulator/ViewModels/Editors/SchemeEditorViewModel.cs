using LogicSimulator.Infrastructure.Attributes;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.ViewModels.Anchorable;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;

namespace LogicSimulator.ViewModels.Editors;

[Editor(typeof(SchemeViewModel))]
public class SchemeEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Схема")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства сетки")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Шаг")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(SchemeViewModel.GridStep), GridStepConfigure))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Ширина")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(SchemeViewModel.GridWidth), SizeStepConfigure))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Высота")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(SchemeViewModel.GridHeight), SizeStepConfigure)))
        .Build();

    private static void GridStepConfigure(NumberPropertyViewModel<float> prop)
    {
        ConfigureAsSizeNumber(prop);
        prop.MaxNumber = 100;
        prop.MinNumber = 0.5f;
    }

    private static void SizeStepConfigure(NumberPropertyViewModel<float> prop)
    {
        ConfigureAsSizeNumber(prop);
        prop.MaxNumber = 10000;
        prop.MinNumber = 100;
    }
}
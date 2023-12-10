using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base.Properties;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

namespace LogicSimulator.ViewModels.EditorViewModels;

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
                .WithSingleProperty<FloatPropertyViewModel>(nameof(SchemeViewModel.GridStep)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Ширина")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(SchemeViewModel.GridWidth)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Высота")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(SchemeViewModel.GridHeight))))
        .Build();
}
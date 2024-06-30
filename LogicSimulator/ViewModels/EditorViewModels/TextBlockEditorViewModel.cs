using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base.Properties;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using LogicSimulator.ViewModels.ObjectViewModels;

namespace LogicSimulator.ViewModels.EditorViewModels;

[Editor(typeof(TextBlockViewModel))]
public class TextBlockEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Текст")
        .WithLocationRotationGroup()
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Текст")
                .WithSingleProperty<StringPropertyViewModel>(nameof(TextBlockViewModel.Text), prop => prop.IsMultiline = true))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Шрифт")
                .WithSingleProperty<FontNamePropertyViewModel>(nameof(TextBlockViewModel.FontName))
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(TextBlockViewModel.FontSize), ConfigureAsFontSizeNumber)
                .WithSingleProperty<ColorPropertyViewModel>(nameof(TextBlockViewModel.TextColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithRelativeSize(0.3)
                    .WithAutoSize()))
            .WithRow(rowBuilder => rowBuilder.
                WithMultiProperty<FontPropertiesViewModel>(multiPropertyBuilder => multiPropertyBuilder
                    .WithProperty<BoolPropertyViewModel>(nameof(TextBlockViewModel.IsBold))
                    .WithProperty<BoolPropertyViewModel>(nameof(TextBlockViewModel.IsItalic))
                    .WithProperty<BoolPropertyViewModel>(nameof(TextBlockViewModel.IsUnderlined))
                    .WithProperty<BoolPropertyViewModel>(nameof(TextBlockViewModel.IsCross)))))
        .Build();
}
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class TextBlockEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Текст")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(TextBlock.Location)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Поворот")
                .WithSingleProperty<RotationEnumPropertyViewModel>(nameof(TextBlock.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Текст")
                .WithSingleProperty<StringPropertyViewModel>(nameof(TextBlock.Text)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Шрифт")
                .WithSingleProperty<FontNamePropertyViewModel>(nameof(TextBlock.FontName))
                .WithSingleProperty<FloatPropertyViewModel>(nameof(TextBlock.FontSize))
                .WithSingleProperty<Color4PropertyViewModel>(nameof(TextBlock.TextColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithRelativeSize(0.3)
                    .WithAutoSize()))
            .WithRow(rowBuilder => rowBuilder.
                WithMultiProperty<FontPropertyViewModel>(multiPropertyBuilder => multiPropertyBuilder
                    .WithProperty<BoolPropertyViewModel>(nameof(TextBlock.IsBold))
                    .WithProperty<BoolPropertyViewModel>(nameof(TextBlock.IsItalic))
                    .WithProperty<BoolPropertyViewModel>(nameof(TextBlock.IsUnderlined))
                    .WithProperty<BoolPropertyViewModel>(nameof(TextBlock.IsCross)))))
        .Build();
}
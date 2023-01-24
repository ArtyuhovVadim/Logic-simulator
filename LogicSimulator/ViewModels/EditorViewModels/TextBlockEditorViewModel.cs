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
                .WithProperty<Vector2PropertyViewModel>(nameof(Rectangle.Location)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Поворот")
                .WithProperty<RotationEnumPropertyViewModel>(nameof(Rectangle.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Текст")
                .WithProperty<StringPropertyViewModel>(nameof(TextBlock.Text)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Шрифт")
                .WithProperty<StringPropertyViewModel>(nameof(TextBlock.FontName))
                .WithProperty<FloatPropertyViewModel>(nameof(TextBlock.FontSize))
                .WithProperty<Color4PropertyViewModel>(nameof(TextBlock.TextColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithRelativeSize(0.3)
                    .WithAutoSize()))
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<BoolPropertyViewModel>(nameof(TextBlock.IsBold))
                .WithProperty<BoolPropertyViewModel>(nameof(TextBlock.IsItalic))
                .WithProperty<BoolPropertyViewModel>(nameof(TextBlock.IsUnderlined))
                .WithProperty<BoolPropertyViewModel>(nameof(TextBlock.IsCross))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithRelativeSize(1)
                    .WithRelativeSize(1)
                    .WithRelativeSize(1))))
        .Build();
}
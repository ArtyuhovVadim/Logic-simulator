﻿using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base.Properties;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using LogicSimulator.ViewModels.ObjectViewModels;

namespace LogicSimulator.ViewModels.EditorViewModels;

[Editor(typeof(ImageViewModel))]
public class ImageEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Изображение")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(ImageViewModel.Location), ConfigureAsPositionVector))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Поворот")
                .WithSingleProperty<EnumPropertyViewModel>(nameof(ImageViewModel.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Путь до файла")
                .WithSingleProperty<StringPropertyViewModel>(nameof(ImageViewModel.FilePath)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Ширина")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(ImageViewModel.Width), ConfigureAsSizeNumber))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Высота")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(ImageViewModel.Height), ConfigureAsSizeNumber))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithSingleProperty<BoolPropertyViewModel>(nameof(ImageViewModel.IsBordered))
                .WithMultiProperty<StrokePropertiesViewModel>(multiPropertyBuilder => multiPropertyBuilder
                    .WithProperty<EnumPropertyViewModel>(nameof(ImageViewModel.StrokeThicknessType))
                    .WithProperty<FloatPropertyViewModel>(nameof(ImageViewModel.StrokeThickness), ConfigureAsSizeNumber)
                    .WithProperty<ColorPropertyViewModel>(nameof(ImageViewModel.StrokeColor)))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithAutoSize()
                    .WithRelativeSize(1))))
        .Build();
}
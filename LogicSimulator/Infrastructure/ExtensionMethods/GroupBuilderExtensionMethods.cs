using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;

namespace LogicSimulator.Infrastructure.ExtensionMethods;

public static class GroupBuilderExtensionMethods
{
    public static GroupBuilder WithBorderRow(this GroupBuilder builder) => builder
        .WithRow(rowBuilder => rowBuilder
            .WithRowName("Граница")
            .WithMultiProperty<StrokePropertiesViewModel>(multiPropertyBuilder => multiPropertyBuilder
                .WithProperty<EnumPropertyViewModel>("StrokeThicknessType")
                .WithProperty<NumberPropertyViewModel<float>>("StrokeThickness", EditorViewModel.ConfigureAsSizeNumber)
                .WithProperty<ColorPropertyViewModel>("StrokeColor")));

    public static GroupBuilder WithFillRow(this GroupBuilder builder) => builder
        .WithRow(rowBuilder => rowBuilder
            .WithRowName("Цвет заливки")
            .WithSingleProperty<ColorPropertyViewModel>("FillColor")
            .WithSingleProperty<BoolPropertyViewModel>("IsFilled")
            .WithLayout(layoutBuilder => layoutBuilder
                .WithAutoSize()
                .WithAutoSize()));

    public static GroupBuilder WithSizeRow(this GroupBuilder builder) => builder
        .WithRow(rowBuilder => rowBuilder
            .WithRowName("Ширина")
            .WithSingleProperty<NumberPropertyViewModel<float>>("Width", EditorViewModel.ConfigureAsSizeNumber))
        .WithRow(rowBuilder => rowBuilder
            .WithRowName("Высота")
            .WithSingleProperty<NumberPropertyViewModel<float>>("Height", EditorViewModel.ConfigureAsSizeNumber));

    public static GroupBuilder WithRadiusRow(this GroupBuilder builder) => builder
        .WithRow(rowBuilder => rowBuilder
            .WithRowName("Радиус X")
            .WithSingleProperty<NumberPropertyViewModel<float>>("RadiusX", EditorViewModel.ConfigureAsSizeNumber))
        .WithRow(rowBuilder => rowBuilder
            .WithRowName("Радиус Y")
            .WithSingleProperty<NumberPropertyViewModel<float>>("RadiusY", EditorViewModel.ConfigureAsSizeNumber));
}
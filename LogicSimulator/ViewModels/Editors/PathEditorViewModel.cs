using LogicSimulator.Infrastructure.Attributes;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;
using LogicSimulator.ViewModels.Objects;

namespace LogicSimulator.ViewModels.Editors;

[Editor(typeof(PathViewModel))]
public class PathEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Путь")
        .WithLocationRotationGroup()
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Геометрия")
                .WithSingleProperty<StringPropertyViewModel>(nameof(PathViewModel.Geometry),
                    prop => prop.IsMultiline = true))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Точка отсчета")
                .WithSingleProperty<EnumPropertyViewModel>(nameof(PathViewModel.OriginPosition)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Ширина")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(PathViewModel.Width), prop =>
                {
                    ConfigureAsSizeNumber(prop);
                    prop.IsNanAllowed = true;
                }))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Высота")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(PathViewModel.Height), prop =>
                {
                    ConfigureAsSizeNumber(prop);
                    prop.IsNanAllowed = true;
                }))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Масштаб")
                .WithSingleProperty<NumberPropertyViewModel<float>>(nameof(PathViewModel.Scale), prop => prop.IsNanAllowed = true)))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вид")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Тип заполнения")
                .WithSingleProperty<EnumPropertyViewModel>(nameof(PathViewModel.Stretch)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithMultiProperty<StrokePropertiesViewModel>(multiPropertyBuilder => multiPropertyBuilder
                    .WithProperty<EnumPropertyViewModel>(nameof(PathViewModel.StrokeThicknessType))
                    .WithProperty<NumberPropertyViewModel<float>>(nameof(PathViewModel.StrokeThickness), ConfigureAsSizeNumber)
                    .WithProperty<ColorPropertyViewModel>(nameof(PathViewModel.StrokeColor)))
                .WithSingleProperty<BoolPropertyViewModel>(nameof(PathViewModel.IsStroked))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize()))
            .WithFillRow()
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Антиалиасинг")
                .WithSingleProperty<BoolPropertyViewModel>(nameof(PathViewModel.IsAntiAliased))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithAutoSize()
                    .WithRelativeSize(1))))
        .Build();
}
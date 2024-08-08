using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;
using LogicSimulator.ViewModels.Objects.Base;

namespace LogicSimulator.Infrastructure.ExtensionMethods;

public static class LayoutBuilderExtensionMethods
{
    public static LayoutBuilder WithLocationRotationGroup(this LayoutBuilder builder) => builder
        .WithGroup(groupBuilder => groupBuilder
        .WithGroupName("Расположение")
        .WithRow(rowBuilder => rowBuilder
            .WithRowName("(X/Y)")
            .WithSingleProperty<Vector2PropertyViewModel>(nameof(BaseObjectViewModel.Location), EditorViewModel.ConfigureAsPositionVector))
        .WithRow(rowBuilder => rowBuilder
            .WithRowName("Поворот")
            .WithSingleProperty<EnumPropertyViewModel>(nameof(BaseObjectViewModel.Rotation))));
}
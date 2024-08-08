using LogicSimulator.Infrastructure.Attributes;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.Infrastructure.EditorLayout.Builders;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.ViewModels.Editors.Base;
using LogicSimulator.ViewModels.Editors.Base.Properties;
using LogicSimulator.ViewModels.Objects;

namespace LogicSimulator.ViewModels.Editors;

[Editor(typeof(BezierCurveViewModel))]
public class BezierCurveEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Кривая Безье")
        .WithLocationRotationGroup()
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вершины")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(BezierCurveViewModel.Point1), ConfigureAsPositionVector))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(BezierCurveViewModel.Point2), ConfigureAsPositionVector))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(BezierCurveViewModel.Point3), ConfigureAsPositionVector)))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithBorderRow())
        .Build();
}
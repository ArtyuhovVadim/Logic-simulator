using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class LineEditorViewModel : EditorViewModel
{
    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create(this)
        .WithName("Ломаная линия")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("(X/Y)")
                .WithSingleProperty<Vector2PropertyViewModel>(nameof(Line.Location)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Поворот")
                .WithSingleProperty<RotationEnumPropertyViewModel>(nameof(Line.Rotation))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Вершины")
            .WithRow(rowBuilder => rowBuilder
                .WithSingleProperty<VerticesPropertyViewModel>(nameof(Line.Vertexes))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithSingleProperty<FloatPropertyViewModel>(nameof(Line.StrokeThickness))
                .WithSingleProperty<Color4PropertyViewModel>(nameof(Line.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize())))
        .Build();
}
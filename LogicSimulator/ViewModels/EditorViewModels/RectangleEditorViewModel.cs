using System;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RowBuilder
{
    private readonly EditorRow _row;

    public RowBuilder() => _row = new EditorRow();

    public RowBuilder WithRowName(string name)
    {
        _row.Name = name;
        return this;
    }

    public RowBuilder WithProperty<T>(string name)
    {
        _row.ObjectProperties.Add(new ObjectProperty(name, typeof(T)));
        return this;
    }

    public EditorRow Build() => _row;
}

public class GroupBuilder
{
    private readonly EditorGroup _group;

    public GroupBuilder() => _group = new EditorGroup();

    public GroupBuilder WithGroupName(string name)
    {
        _group.Name = name;
        return this;
    }

    public GroupBuilder WithRow(Action<RowBuilder> configureAction)
    {
        var rowBuilder = new RowBuilder();
        configureAction(rowBuilder);
        _group.EditorRows.Add(rowBuilder.Build());
        return this;
    }

    public EditorGroup Build() => _group;
}

public class LayoutBuilder
{
    private readonly EditorLayout _layout;

    private LayoutBuilder() => _layout = new EditorLayout
    {
        ObjectName = string.Empty,
    };

    public static LayoutBuilder Create() => new();

    public LayoutBuilder WithName(string name)
    {
        _layout.ObjectName = name;
        return this;
    }

    public LayoutBuilder WithGroup(Action<GroupBuilder> configureAction)
    {
        var groupBuilder = new GroupBuilder();
        configureAction.Invoke(groupBuilder);
        _layout.Groups.Add(groupBuilder.Build());
        return this;
    }

    public EditorLayout Build() => _layout;
}

public class RectangleEditorViewModel : BaseEditorViewModel<Rectangle>
{
    public Vector2 Location { get => Get<Vector2>(); set => Set(value); }
    public float Width { get => Get<float>(); set => Set(value); }
    public float Height { get => Get<float>(); set => Set(value); }
    public float StrokeThickness { get => Get<float>(); set => Set(value); }
    public Color4 StrokeColor { get => Get<Color4>(); set => Set(value); }
    public bool IsFilled { get => Get<bool>(); set => Set(value); }
    public Color4 FillColor { get => Get<Color4>(); set => Set(value); }

    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create()
        .WithName("Прямоугольник")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<Vector2>(nameof(Rectangle.Location))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Ширина")
                .WithProperty<float>(nameof(Rectangle.Width)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Высота")
                .WithProperty<float>(nameof(Rectangle.Height)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithProperty<float>(nameof(Rectangle.StrokeThickness))
                .WithProperty<Color4>(nameof(Rectangle.StrokeColor)))
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Цвет заливки")
                .WithProperty<Color4>(nameof(Rectangle.FillColor))
                .WithProperty<bool>(nameof(Rectangle.IsFilled))))
        .Build();
}
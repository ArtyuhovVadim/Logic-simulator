using System;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

public class LayoutBuilder
{
    private readonly EditorLayout _layoutViewModel;

    private LayoutBuilder() => _layoutViewModel = new EditorLayout
    {
        ObjectName = string.Empty,
    };

    public static LayoutBuilder Create() => new();

    public LayoutBuilder WithName(string name)
    {
        _layoutViewModel.ObjectName = name;
        return this;
    }

    public LayoutBuilder WithGroup(Action<GroupBuilder> configureAction)
    {
        var groupBuilder = new GroupBuilder();
        configureAction.Invoke(groupBuilder);
        _layoutViewModel.Groups.Add(groupBuilder.Build());
        return this;
    }

    public EditorLayout Build() => _layoutViewModel;
}
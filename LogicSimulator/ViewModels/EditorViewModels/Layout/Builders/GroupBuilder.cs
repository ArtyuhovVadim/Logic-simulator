using System;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

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
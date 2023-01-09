using System;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

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

    public RowBuilder WithLayout(Action<RowLayoutBuilder> configureAction)
    {
        var builder = new RowLayoutBuilder();
        configureAction(builder);
        _row.Layout = builder.Build();
        return this;
    }

    public EditorRow Build() => _row;
}
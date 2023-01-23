using System;
using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

public class RowBuilder
{
    private readonly EditorRow _row;

    public RowBuilder(EditorViewModel editorViewModel) => _row = new EditorRow { EditorViewModel = editorViewModel };

    public RowBuilder WithRowName(string name)
    {
        _row.Name = name;
        return this;
    }

    public RowBuilder WithProperty<T>(string name) where T : PropertyViewModel, new()
    {
        _row.ObjectProperties.Add(new T
        {
            EditorViewModel = _row.EditorViewModel,
            PropertyName = name
        });
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
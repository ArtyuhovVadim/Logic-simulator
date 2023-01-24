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

    public RowBuilder WithMultiProperty<T>(Action<MultiPropertyBuilder<T>> configureAction) where T : MultiPropertyViewModel, new()
    {
        var builder = new MultiPropertyBuilder<T>(_row.EditorViewModel);
        configureAction.Invoke(builder);
        _row.ObjectProperties.Add(builder.Build());
        return this;
    }

    public RowBuilder WithSingleProperty<T>(string name) where T : SinglePropertyViewModel, new()
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
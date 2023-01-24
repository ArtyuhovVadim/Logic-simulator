using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

public class MultiPropertyBuilder<T> where T : MultiPropertyViewModel, new()
{
    private readonly T _multiProperty;

    public MultiPropertyBuilder(EditorViewModel editorViewModel) => _multiProperty = new T { EditorViewModel = editorViewModel };

    public MultiPropertyBuilder<T> WithProperty<TProp>(string name) where TProp : SinglePropertyViewModel, new()
    {
        _multiProperty.AddProperty<TProp>(name);

        return this;
    }

    public MultiPropertyViewModel Build() => _multiProperty;
}
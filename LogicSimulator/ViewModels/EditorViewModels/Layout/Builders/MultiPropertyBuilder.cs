using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

public class MultiPropertyBuilder<T> where T : MultiPropertyViewModel, new()
{
    private readonly T _multiProperty;

    public MultiPropertyBuilder(EditorViewModel editorViewModel) => _multiProperty = new T { EditorViewModel = editorViewModel };

    public MultiPropertyBuilder<T> WithProperty<TPropertyViewModel>(string name) where TPropertyViewModel : SinglePropertyViewModel, new()
    {
        _multiProperty.AddProperty<TPropertyViewModel>(name);
        return this;
    }

    public MultiPropertyBuilder<T> WithProperty<TPropertyViewModel>(string name, Action<TPropertyViewModel> configureAction) where TPropertyViewModel : SinglePropertyViewModel, new()
    {
        _multiProperty.AddProperty(name, configureAction);
        return this;
    }

    public MultiPropertyViewModel Build() => _multiProperty;
}
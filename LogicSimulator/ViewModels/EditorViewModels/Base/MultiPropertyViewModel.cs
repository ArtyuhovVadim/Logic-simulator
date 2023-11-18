namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class MultiPropertyViewModel : PropertyViewModel
{
    private readonly Dictionary<string, SinglePropertyViewModel> _properties = new();

    public IReadOnlyDictionary<string, SinglePropertyViewModel> Properties => _properties;

    public void AddProperty<T>(string name) where T : SinglePropertyViewModel, new()
    {
        _properties.Add(name, new T
        {
            EditorViewModel = EditorViewModel,
            PropertyName = name
        });
    }

    public override void ProvidePropertyChanged(string propName)
    {
        if (_properties.TryGetValue(propName, out var property))
        {
            property.ProvidePropertyChanged(propName);
        }
    }
}
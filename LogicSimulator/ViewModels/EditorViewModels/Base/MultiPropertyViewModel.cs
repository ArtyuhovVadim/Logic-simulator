namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class MultiPropertyViewModel : PropertyViewModel
{
    private readonly Dictionary<string, SinglePropertyViewModel> _properties = [];

    public IReadOnlyDictionary<string, SinglePropertyViewModel> Properties => _properties;

    public void AddProperty<TPropertyViewModel>(string name) where TPropertyViewModel : SinglePropertyViewModel, new()
    {
        _properties.Add(name, new TPropertyViewModel
        {
            EditorViewModel = EditorViewModel,
            PropertyName = name
        });
    }

    public void AddProperty<TPropertyViewModel>(string name, Action<TPropertyViewModel> configureAction) where TPropertyViewModel : SinglePropertyViewModel, new()
    {
        var prop = new TPropertyViewModel
        {
            EditorViewModel = EditorViewModel,
            PropertyName = name
        };

        configureAction.Invoke(prop);

        _properties.Add(name, prop);
    }

    public override void ProvidePropertyChanged(string propName)
    {
        if (_properties.TryGetValue(propName, out var property))
        {
            property.ProvidePropertyChanged(propName);
        }
    }

    public override void RaisePropertyChanged()
    {
        foreach (var property in _properties.Values)
        {
            property.RaisePropertyChanged();
        }
    }

    protected void CopySinglePropertiesToOther(MultiPropertyViewModel other)
    {
        foreach (var prop in _properties)
        {
            other._properties[prop.Key] = (SinglePropertyViewModel)prop.Value.MakeCopy(other.EditorViewModel);
        }
    }

    protected override void OnStartEdit(IEnumerable<object> objects)
    {
        foreach (var property in _properties.Values)
        {
            property.StartEdit();
        }
    }

    protected override void OnEndEdit(IEnumerable<object> objects)
    {
        foreach (var property in _properties.Values)
        {
            property.EndEdit();
        }
    }
}
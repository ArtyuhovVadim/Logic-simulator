using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorRow : BindableBase
{
    private string _name = string.Empty;
    private ObservableCollection<ObjectProperty> _objectProperties = new();

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    public ObservableCollection<ObjectProperty> ObjectProperties
    {
        get => _objectProperties;
        set => Set(ref _objectProperties, value);
    }
}
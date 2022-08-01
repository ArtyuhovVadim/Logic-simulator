using System.Collections.ObjectModel;
using LogicSimulator.Models;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public class SchemeViewModel : BindableBase
{
    private readonly Scheme _scheme;

    public SchemeViewModel(Scheme scheme)
    {
        _scheme = scheme;

        Objects = new ObservableCollection<BaseSceneObject>(_scheme.Objects);
    }

    #region Name

    public string Name
    {
        get => _scheme.Name;
        set
        {
            _scheme.Name = value; 
            OnPropertyChanged();
        }
    }

    #endregion

    #region Objects

    private ObservableCollection<BaseSceneObject> _objects;

    public ObservableCollection<BaseSceneObject> Objects
    {
        get => _objects;
        set => Set(ref _objects, value);
    }

    #endregion
}
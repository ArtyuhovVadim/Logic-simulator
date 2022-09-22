using System.Collections.Generic;
using System.ComponentModel;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class AbstractEditorViewModel : BindableBase
{
    public abstract void SetObjectsToEdit(IEnumerable<BaseSceneObject> objects);

    protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e.PropertyName);
}
using System.Collections.Generic;
using System.ComponentModel;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class AbstractEditorViewModel : BindableBase
{
    private EditorLayout _layout;

    public EditorLayout Layout => _layout ??= CreateLayout();

    public abstract void SetObjectsToEdit(IEnumerable<BaseSceneObject> objects);

    protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e.PropertyName);

    protected abstract EditorLayout CreateLayout();

    protected ObjectProperty CreateObjectPropertyViewModel<T>(string name) =>
        //new(name, typeof(T), s => Get<T>(s), (v, s) => Set((T)v, s));
        new(name, typeof(T));
}
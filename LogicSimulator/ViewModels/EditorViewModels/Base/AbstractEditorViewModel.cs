using System.Collections.Generic;
using System.ComponentModel;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class AbstractEditorViewModel : BindableBase
{
    private EditorLayout _layoutViewModel;

    public EditorLayout LayoutViewModel => _layoutViewModel ??= CreateLayout();

    public Dictionary<string, bool> UndefinedPropertiesMap { get; } = new();

    public abstract void SetObjectsToEdit(IEnumerable<BaseSceneObject> objects);

    protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => LayoutViewModel.PropertyChange(e.PropertyName);

    protected abstract EditorLayout CreateLayout();
}
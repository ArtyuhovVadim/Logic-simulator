using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class PropertyViewModel : BindableBase
{
    public EditorViewModel EditorViewModel { get; set; }

    public abstract void ProvidePropertyChanged(string propName);

    public void StartEdit() => OnStartEdit(EditorViewModel.Objects);

    public void EndEdit() => OnEndEdit(EditorViewModel.Objects);

    protected virtual void OnStartEdit(IEnumerable<object> objects) { }

    protected virtual void OnEndEdit(IEnumerable<object> objects) { }
}
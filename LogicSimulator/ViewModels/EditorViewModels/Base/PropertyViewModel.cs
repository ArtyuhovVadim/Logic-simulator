using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class PropertyViewModel : BindableBase
{
    public EditorViewModel EditorViewModel { get; set; } = null!;

    public abstract void ProvidePropertyChanged(string propName);

    public abstract void RaisePropertyChanged();

    public void StartEdit() => OnStartEdit(EditorViewModel.Objects);

    public void EndEdit() => OnEndEdit(EditorViewModel.Objects);

    public abstract PropertyViewModel MakeCopy(EditorViewModel editor);

    protected virtual void OnStartEdit(IEnumerable<object> objects) { }

    protected virtual void OnEndEdit(IEnumerable<object> objects) { }
}
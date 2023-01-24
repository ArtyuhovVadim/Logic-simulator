using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class PropertyViewModel : BindableBase
{
    public EditorViewModel EditorViewModel { get; set; }

    public abstract void ProvidePropertyChanged(string propName);
}
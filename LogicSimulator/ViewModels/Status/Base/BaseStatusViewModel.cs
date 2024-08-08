using LogicSimulator.ViewModels.Anchorable.Base;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.Status.Base;

public abstract class BaseStatusViewModel : BindableBase
{
    protected BaseStatusViewModel(DocumentViewModel parent) { }

    public void RaisedPropertyChanged(string propName) => OnPropertyChanged(propName);
}
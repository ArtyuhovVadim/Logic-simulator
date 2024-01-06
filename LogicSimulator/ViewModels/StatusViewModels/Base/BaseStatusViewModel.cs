using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.StatusViewModels.Base;

public abstract class BaseStatusViewModel : BindableBase
{
    protected BaseStatusViewModel(DocumentViewModel parent) { }

    public void RaisedPropertyChanged(string propName) => OnPropertyChanged(propName);
}
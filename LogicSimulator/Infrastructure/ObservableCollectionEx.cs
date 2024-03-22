using WpfExtensions.Mvvm;

namespace LogicSimulator.Infrastructure;

public class ObservableCollectionEx<TViewModel, TModel> : SynchronizedObservableCollection<TViewModel, TModel> where TViewModel : BindableBase, IModelBased<TModel>
{
    public ObservableCollectionEx(IList<TModel> modelList, Func<TModel, TViewModel> viewModelCreateFunc) :
        base(modelList, viewModelCreateFunc, vm => vm.Model)
    { }
}
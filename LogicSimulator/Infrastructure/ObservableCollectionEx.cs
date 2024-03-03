using System.Collections.Specialized;
using System.Windows.Automation;
using WpfExtensions.Mvvm;

namespace LogicSimulator.Infrastructure;

public class ObservableCollectionEx<TViewModel, TModel> : ObservableCollection<TViewModel> where TViewModel : BindableBase, IModelBased<TModel>
{
    private readonly IList<TModel> _modelList;
    private readonly bool _suppressCollectionChanged;

    public ObservableCollectionEx(IList<TModel> modelList, Func<TModel, TViewModel> viewModelCreateFunc)
    {
        _modelList = modelList;

        _suppressCollectionChanged = true;
        foreach (var model in modelList)
            Add(viewModelCreateFunc(model));
        _suppressCollectionChanged = false;
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        if (_suppressCollectionChanged)
            return;

        switch (args.Action)
        {
            case NotifyCollectionChangedAction.Add: HandleAdd(args); break;
            case NotifyCollectionChangedAction.Remove: HandleRemove(args); break;
            case NotifyCollectionChangedAction.Replace: HandleReplace(args); break;
            case NotifyCollectionChangedAction.Move: HandleMove(args); break;
            case NotifyCollectionChangedAction.Reset: HandleReset(args); break;
            default: throw new ArgumentOutOfRangeException();
        }

        base.OnCollectionChanged(args);
    }

    private void HandleAdd(NotifyCollectionChangedEventArgs args)
    {
        if (args.NewItems!.Count > 1)
            throw new InvalidOperationException();

        if (args.NewStartingIndex == Count - 1)
        {
            // Add to end of collection
            _modelList.Add(((TViewModel)args.NewItems[0]!).Model);
        }
        else
        {
            // Add to 'args.NewStartingIndex' index of collection
            _modelList.Insert(args.NewStartingIndex, ((TViewModel)args.NewItems[0]!).Model);
        }
    }

    private void HandleRemove(NotifyCollectionChangedEventArgs args)
    {
        if (args.OldItems!.Count > 1)
            throw new InvalidOperationException();

        if (!_modelList.Remove(((TViewModel)args.OldItems[0]!).Model))
            throw new ElementNotAvailableException("Item not found.");
    }

    private void HandleReplace(NotifyCollectionChangedEventArgs args)
    {
        if (args.OldItems!.Count > 1 || args.NewItems!.Count > 1)
            throw new InvalidOperationException();

        _modelList[args.NewStartingIndex] = ((TViewModel)args.NewItems[0]!).Model;
    }

    private void HandleMove(NotifyCollectionChangedEventArgs args)
    {
        if (args.NewItems!.Count > 1)
            throw new InvalidOperationException();

        var model = ((TViewModel)args.NewItems[0]!).Model;

        if (!_modelList.Remove(model))
            throw new ElementNotAvailableException("Item not found.");

        _modelList.Insert(args.NewStartingIndex, model);

    }

    private void HandleReset(NotifyCollectionChangedEventArgs args)
    {
       _modelList.Clear();
    }
}
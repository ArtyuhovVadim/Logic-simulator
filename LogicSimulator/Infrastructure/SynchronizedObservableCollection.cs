using System.Collections.Specialized;
using System.Windows.Automation;

namespace LogicSimulator.Infrastructure;

public class SynchronizedObservableCollection<TSourceItem, TRecipientItem> : ObservableCollection<TSourceItem>
{
    private readonly IList<TRecipientItem> _collectionToSynchronize;
    private readonly Func<TSourceItem, TRecipientItem> _sourceToRecipientItem;
    private readonly bool _suppressCollectionChanged;

    public SynchronizedObservableCollection(IList<TRecipientItem> collectionToSynchronize, Func<TRecipientItem, TSourceItem> recipientToSourceItem, Func<TSourceItem, TRecipientItem> sourceToRecipientItem)
    {
        _collectionToSynchronize = collectionToSynchronize;
        _sourceToRecipientItem = sourceToRecipientItem;

        _suppressCollectionChanged = true;
        foreach (var model in collectionToSynchronize)
            Add(recipientToSourceItem(model));
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
            _collectionToSynchronize.Add(_sourceToRecipientItem((TSourceItem)args.NewItems[0]!));
        }
        else
        {
            // Add to 'args.NewStartingIndex' index of collection
            _collectionToSynchronize.Insert(args.NewStartingIndex, _sourceToRecipientItem((TSourceItem)args.NewItems[0]!));
        }
    }

    private void HandleRemove(NotifyCollectionChangedEventArgs args)
    {
        if (args.OldItems!.Count > 1)
            throw new InvalidOperationException();

        if (!_collectionToSynchronize.Remove(_sourceToRecipientItem((TSourceItem)args.OldItems[0]!)))
            throw new ElementNotAvailableException("Item not found.");
    }

    private void HandleReplace(NotifyCollectionChangedEventArgs args)
    {
        if (args.OldItems!.Count > 1 || args.NewItems!.Count > 1)
            throw new InvalidOperationException();

        _collectionToSynchronize[args.NewStartingIndex] = _sourceToRecipientItem((TSourceItem)args.NewItems[0]!);
    }

    private void HandleMove(NotifyCollectionChangedEventArgs args)
    {
        if (args.NewItems!.Count > 1)
            throw new InvalidOperationException();

        var model = _sourceToRecipientItem((TSourceItem)args.NewItems[0]!);

        if (!_collectionToSynchronize.Remove(model))
            throw new ElementNotAvailableException("Item not found.");

        _collectionToSynchronize.Insert(args.NewStartingIndex, model);

    }

    private void HandleReset(NotifyCollectionChangedEventArgs args)
    {
        _collectionToSynchronize.Clear();
    }
}
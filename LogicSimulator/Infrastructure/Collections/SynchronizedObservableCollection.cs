using System.Collections.Specialized;
using System.ComponentModel;

namespace LogicSimulator.Infrastructure.Collections;

public class SynchronizedObservableCollection<TSourceItem, TRecipientItem> : ObservableCollection<TSourceItem>
{
    private readonly IList<TRecipientItem> _collectionToSynchronize;
    private readonly Func<TRecipientItem, TSourceItem> _recipientToSourceItem;
    private readonly Func<TSourceItem, TRecipientItem> _sourceToRecipientItem;
    private bool _suppressCollectionChanged;

    public SynchronizedObservableCollection(IList<TRecipientItem> collectionToSynchronize, Func<TRecipientItem, TSourceItem> recipientToSourceItem, Func<TSourceItem, TRecipientItem> sourceToRecipientItem)
    {
        _collectionToSynchronize = collectionToSynchronize;
        _recipientToSourceItem = recipientToSourceItem;
        _sourceToRecipientItem = sourceToRecipientItem;

        _suppressCollectionChanged = true;
        foreach (var model in collectionToSynchronize)
            Add(recipientToSourceItem(model));
        _suppressCollectionChanged = false;
    }

    public void Add(TRecipientItem item) => Add(_recipientToSourceItem(item));

    public void AddRange(IEnumerable<TSourceItem> items)
    {
        var count = Count;
        _suppressCollectionChanged = true;
        var itemsList = items.ToList();
        foreach (var item in itemsList)
            Add(item);
        _suppressCollectionChanged = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, itemsList, count));
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
    }

    public void AddRange(IEnumerable<TRecipientItem> items)
    {
        var count = Count;
        _suppressCollectionChanged = true;
        var itemsList = items.Select(_recipientToSourceItem).ToList();
        foreach (var item in itemsList)
            Add(item);
        _suppressCollectionChanged = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, itemsList, count));
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
    }

    public int RemoveAll(Predicate<TSourceItem> predicate)
    {
        var removed = 0;

        for (var i = Items.Count - 1; i >= 0; i--)
        {
            if (predicate(Items[i]))
            {
                RemoveAt(i);
                removed++;
            }
        }

        return removed;
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
            case NotifyCollectionChangedAction.Reset: HandleReset(); break;
            default: throw new ArgumentOutOfRangeException();
        }

        base.OnCollectionChanged(args);
    }

    private void HandleAdd(NotifyCollectionChangedEventArgs args)
    {
        if (args.NewStartingIndex == Count - 1)
        {
            // Add to end of collection
            foreach (var item in args.NewItems!.Cast<TSourceItem>())
            {
                _collectionToSynchronize.Add(_sourceToRecipientItem(item));
            }
        }
        else
        {
            // Add to 'args.NewStartingIndex' index of collection
            var i = 0;
            foreach (var item in args.NewItems!.Cast<TSourceItem>())
            {
                _collectionToSynchronize.Insert(args.NewStartingIndex + i, _sourceToRecipientItem(item));
                i++;
            }
        }
    }

    private void HandleRemove(NotifyCollectionChangedEventArgs args)
    {
        if (args.OldItems!.Count > 1)
            throw new InvalidOperationException();

        if (!_collectionToSynchronize.Remove(_sourceToRecipientItem((TSourceItem)args.OldItems[0]!)))
            throw new InvalidOperationException("Item not found.");
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
            throw new InvalidOperationException("Item not found.");

        _collectionToSynchronize.Insert(args.NewStartingIndex, model);
    }

    private void HandleReset() => _collectionToSynchronize.Clear();
}
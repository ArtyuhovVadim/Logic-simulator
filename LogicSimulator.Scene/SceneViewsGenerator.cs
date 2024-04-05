using System.Collections.Specialized;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.Views.Base;

namespace LogicSimulator.Scene;

public class SceneViewsGenerator<T> : IDisposable where T : DependencyObject, ISceneViewsGeneratorHost
{
    private readonly T _parent;
    private readonly DataTemplateSelector _selector;
    private readonly IEnumerable<object> _items;
    private readonly ResourceCache _cache;
    private readonly List<SceneObjectView> _views = [];

    public event Action? ItemsCollectionChanged;

    public SceneViewsGenerator(T parent, DataTemplateSelector selector, IEnumerable<object> items, ResourceCache cache)
    {
        _parent = parent;
        _selector = selector;
        _items = items;
        _cache = cache;

        if (items is INotifyCollectionChanged collection)
        {
            collection.CollectionChanged += OnObjectsCollectionChanged;
        }

        foreach (var item in _items)
        {
            _views.Add(CreateViewFromItem(item));
        }
    }

    public IEnumerable<SceneObjectView> Views => _views;

    public SceneObjectView? GetViewFromItem(object item) =>
        _views.FirstOrDefault(x => x.DataContext == item);

    public void UpdateCache(ResourceCache cache)
    {
        foreach (var view in _views)
        {
            view.InitializeCache(cache);
        }
    }

    private void OnObjectsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add: HandleAdd(e); break;
            case NotifyCollectionChangedAction.Remove: HandleRemove(e); break;
            case NotifyCollectionChangedAction.Replace: HandleReplace(e); break;
            case NotifyCollectionChangedAction.Move: HandleMove(e); break;
            case NotifyCollectionChangedAction.Reset: HandleReset(); break;
            default: throw new ArgumentOutOfRangeException();
        }

        ItemsCollectionChanged?.Invoke();
    }

    private void HandleAdd(NotifyCollectionChangedEventArgs args)
    {
        if (args.NewStartingIndex == _items.Count() - 1)
        {
            // Add to end of collection
            _views.AddRange(args.NewItems!.Cast<object>().Select(CreateViewFromItem));
        }
        else
        {
            // Add to 'args.NewStartingIndex' index of collection  
            _views.InsertRange(args.NewStartingIndex, args.NewItems!.Cast<object>().Select(CreateViewFromItem));
        }
    }

    private void HandleRemove(NotifyCollectionChangedEventArgs args)
    {
        foreach (var item in args.OldItems!)
        {
            var view = GetViewFromItem(item);

            if (view is null)
                throw new ElementNotAvailableException($"View for {item.GetType().Name} item not found.");

            if (!_views.Remove(view))
                throw new ElementNotAvailableException($"View for {item.GetType().Name} item not found.");

            RemoveFromParentLogicalTree(view);
            view.Dispose();
        }
    }

    private void HandleReplace(NotifyCollectionChangedEventArgs args)
    {
        var oldItems = args.OldItems!.Cast<object>();
        var newItems = args.NewItems!.Cast<object>().ToArray();

        foreach (var oldItem in oldItems)
        {
            var view = GetViewFromItem(oldItem);

            if (view is null)
                throw new ElementNotAvailableException($"View for {oldItem.GetType().Name} item not found.");

            RemoveFromParentLogicalTree(view);

            view.Dispose();
        }

        for (var i = 0; i < newItems.Length; i++)
        {
            _views[i + args.NewStartingIndex] = CreateViewFromItem(newItems[i]);
        }
    }

    private void HandleMove(NotifyCollectionChangedEventArgs args)
    {
        foreach (var newItem in args.NewItems!)
        {
            var view = GetViewFromItem(newItem);

            if (view is null)
                throw new ElementNotAvailableException($"View for {newItem.GetType().Name} item not found.");

            if (!_views.Remove(view))
                throw new ElementNotAvailableException($"View for {newItem.GetType().Name} item not found.");

            _views.Insert(args.NewStartingIndex, view);
        }
    }

    private void HandleReset()
    {
        foreach (var view in _views)
        {
            RemoveFromParentLogicalTree(view);
            view.Dispose();
        }

        _views.Clear();
    }

    private SceneObjectView CreateViewFromItem(object item)
    {
        var template = _selector.SelectTemplate(item, _parent);

        if (template is null)
            throw new NotSupportedException($"Template for {item.GetType().Name} is not supported.");

        if (template.LoadContent() is not SceneObjectView view)
            throw new NotSupportedException($"Template root node must be inherited from {nameof(SceneObjectView)}.");

        view.InitializeCache(_cache);
        view.DataContext = item;
        AddToParentLogicalTree(view);

        return view;
    }

    private void AddToParentLogicalTree(DependencyObject view)
    {
        if (LogicalTreeHelper.GetParent(view) is not null)
            throw new InvalidOperationException($"View {view} already attached to logical tree.");

        _parent.AddLogicalChild(view);
    }

    private void RemoveFromParentLogicalTree(DependencyObject view)
    {
        if (LogicalTreeHelper.GetParent(view) != _parent)
            throw new InvalidOperationException($"View {view} is not in logical tree.");

        _parent.RemoveLogicalChild(view);
    }

    public void Dispose()
    {
        foreach (var view in _views)
        {
            RemoveFromParentLogicalTree(view);
            view.Dispose();
        }

        _views.Clear();

        if (_items is INotifyCollectionChanged collection)
        {
            collection.CollectionChanged -= OnObjectsCollectionChanged;
        }
    }
}
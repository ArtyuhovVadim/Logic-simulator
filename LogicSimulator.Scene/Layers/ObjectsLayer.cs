using System.Collections.Specialized;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.Layers.Base;
using LogicSimulator.Scene.Views.Base;

namespace LogicSimulator.Scene.Layers;

public class ObjectsLayer : BaseSceneLayer
{
    private readonly List<SceneObjectView> _objects = new();

    #region ObjectTemplateSelector

    public DataTemplateSelector ObjectTemplateSelector
    {
        get => (DataTemplateSelector)GetValue(ObjectTemplateSelectorProperty);
        set => SetValue(ObjectTemplateSelectorProperty, value);
    }

    public static readonly DependencyProperty ObjectTemplateSelectorProperty =
        DependencyProperty.Register(nameof(ObjectTemplateSelector), typeof(DataTemplateSelector), typeof(ObjectsLayer), new PropertyMetadata(default(DataTemplateSelector)));

    #endregion

    #region Objects

    public IEnumerable<object> Objects
    {
        get => (IEnumerable<object>)GetValue(ObjectsProperty);
        set => SetValue(ObjectsProperty, value);
    }

    public static readonly DependencyProperty ObjectsProperty =
        DependencyProperty.Register(nameof(Objects), typeof(IEnumerable<object>), typeof(ObjectsLayer), new PropertyMetadata(Enumerable.Empty<object>(), OnObjectsChanged));

    private static void OnObjectsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ObjectsLayer layer) return;

        var newValue = (IEnumerable<object>)e.NewValue;
        var oldValue = (IEnumerable<object>)e.OldValue;

        if (oldValue is INotifyCollectionChanged oldCollectionChanged)
        {
            oldCollectionChanged.CollectionChanged -= layer.OnObjectsCollectionChanged;
        }

        if (newValue is INotifyCollectionChanged newCollectionChanged)
        {
            newCollectionChanged.CollectionChanged += layer.OnObjectsCollectionChanged;
        }

        if (!layer.IsLoaded)
            return;

        layer._objects.Clear();

        foreach (var layerObject in layer.Objects)
        {
            layer._objects.Add(layer.CreateViewFromItem(layerObject));
        }
    }

    #endregion

    public ObjectsLayer() => Loaded += OnLoaded;

    public IEnumerable<ISelectionRenderable> Views => _objects;

    public SceneObjectView? GetViewFromItem(object item) =>
        _objects.FirstOrDefault(x => x.DataContext == item);

    protected override bool OnIsDirtyEvaluation() => Views.Any(x => x.IsDirty);

    protected override void OnCacheChanged(ResourceCache cache)
    {
        foreach (var view in _objects)
        {
            view.InitializeCache(cache);
        }

        base.OnCacheChanged(cache);
    }

    protected override void Dispose(bool disposingManaged)
    {
        if (disposingManaged)
        {
            foreach (var view in Views)
            {
                view.Dispose();
            }

            _objects.Clear();

            if (Objects is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged -= OnObjectsCollectionChanged;
            }
        }

        base.Dispose(disposingManaged);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;

        foreach (var layerObject in Objects)
        {
            _objects.Add(CreateViewFromItem(layerObject));
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

        MakeDirty();
    }

    private void HandleAdd(NotifyCollectionChangedEventArgs args)
    {
        if (args.NewStartingIndex == Objects.Count() - 1)
        {
            // Add to end of collection
            _objects.AddRange(args.NewItems!.Cast<object>().Select(CreateViewFromItem));
        }
        else
        {
            // Add to 'args.NewStartingIndex' index of collection  
            _objects.InsertRange(args.NewStartingIndex, args.NewItems!.Cast<object>().Select(CreateViewFromItem));
        }
    }

    private void HandleRemove(NotifyCollectionChangedEventArgs args)
    {
        foreach (var item in args.OldItems!)
        {
            var view = GetViewFromItem(item);

            if (view is null)
                throw new ElementNotAvailableException($"View for {item.GetType().Name} item not found.");

            if (!_objects.Remove(view))
                throw new ElementNotAvailableException($"View for {item.GetType().Name} item not found.");

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

            view.Dispose();
        }

        for (var i = 0; i < newItems.Length; i++)
        {
            _objects[i + args.NewStartingIndex] = CreateViewFromItem(newItems[i]);
        }
    }

    private void HandleMove(NotifyCollectionChangedEventArgs args)
    {
        foreach (var newItem in args.NewItems!)
        {
            var view = GetViewFromItem(newItem);

            if (view is null)
                throw new ElementNotAvailableException($"View for {newItem.GetType().Name} item not found.");

            if (!_objects.Remove(view))
                throw new ElementNotAvailableException($"View for {newItem.GetType().Name} item not found.");

            _objects.Insert(args.NewStartingIndex, view);
        }
    }

    private void HandleReset()
    {
        foreach (var view in _objects)
        {
            view.Dispose();
        }

        _objects.Clear();
    }

    private SceneObjectView CreateViewFromItem(object item)
    {
        var template = ObjectTemplateSelector.SelectTemplate(item, this);

        if (template is null)
            throw new NotSupportedException($"Template for {item.GetType().Name} is not supported.");

        if (template.LoadContent() is not SceneObjectView view)
            throw new NotSupportedException($"Template root node must be inherited from {nameof(SceneObjectView)}.");

        view.InitializeCache(Cache);
        view.DataContext = item;

        return view;
    }
}
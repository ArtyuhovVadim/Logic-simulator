using System.Collections;
using System.Windows;
using System.Windows.Controls;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.Layers.Base;
using LogicSimulator.Scene.Views.Base;

namespace LogicSimulator.Scene.Layers;

public class ObjectsLayer : BaseSceneLayer, ISceneViewsGeneratorHost
{
    private SceneViewsGenerator<ObjectsLayer>? _generator;

    #region ObjectTemplateSelector

    public DataTemplateSelector ObjectTemplateSelector
    {
        get => (DataTemplateSelector)GetValue(ObjectTemplateSelectorProperty);
        set => SetValue(ObjectTemplateSelectorProperty, value);
    }

    public static readonly DependencyProperty ObjectTemplateSelectorProperty =
        DependencyProperty.Register(nameof(ObjectTemplateSelector), typeof(DataTemplateSelector), typeof(ObjectsLayer), new PropertyMetadata(default(DataTemplateSelector), OnObjectTemplateSelectorChanged));

    private static void OnObjectTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ObjectsLayer layer) return;

        if (!layer.IsLoaded) return;

        layer.UpdateGenerator();
    }

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

        if (!layer.IsLoaded) return;

        layer.UpdateGenerator();
    }

    #endregion

    #region Views

    private static readonly DependencyPropertyKey ViewsPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(Views), typeof(IEnumerable<ISelectionRenderable>), typeof(ObjectsLayer), new PropertyMetadata(Enumerable.Empty<ISelectionRenderable>()));

    public static readonly DependencyProperty ViewsProperty = ViewsPropertyKey.DependencyProperty;

    public IEnumerable<ISelectionRenderable> Views
    {
        get => (IEnumerable<ISelectionRenderable>)GetValue(ViewsProperty);
        private set => SetValue(ViewsPropertyKey, value);
    }

    #endregion

    public ObjectsLayer() => Loaded += OnLoaded;

    public SceneObjectView? GetViewFromItem(object item) => _generator?.GetViewFromItem(item);

    protected override IEnumerator LogicalChildren => Views.GetEnumerator();

    protected override bool OnIsDirtyEvaluation() => Views.Any(x => x.IsDirty);

    protected override void OnCacheChanged(ResourceCache cache)
    {
        _generator?.UpdateCache(cache);
        base.OnCacheChanged(cache);
    }

    protected override void Dispose(bool disposingManaged)
    {
        if (disposingManaged)
        {
            if (_generator is not null)
            {
                _generator.Dispose();
                _generator.ItemsCollectionChanged -= OnObjectsCollectionChanged;
            }
        }

        base.Dispose(disposingManaged);
    }

    private void UpdateGenerator()
    {
        if (_generator is not null)
        {
            _generator.ItemsCollectionChanged -= OnObjectsCollectionChanged;
            _generator.Dispose();
        }

        _generator = new SceneViewsGenerator<ObjectsLayer>(this, ObjectTemplateSelector, Objects, Cache);
        _generator.ItemsCollectionChanged += OnObjectsCollectionChanged;
        Views = _generator.Views;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        UpdateGenerator();
    }

    private void OnObjectsCollectionChanged() => MakeDirty();

    void ISceneViewsGeneratorHost.AddLogicalChild(object child) => AddLogicalChild(child);

    void ISceneViewsGeneratorHost.RemoveLogicalChild(object child) => RemoveLogicalChild(child);
}
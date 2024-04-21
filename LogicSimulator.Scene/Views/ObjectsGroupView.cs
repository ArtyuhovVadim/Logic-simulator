using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Views;

[ContentProperty(nameof(Items))]
public class ObjectsGroupView : SceneObjectView, ISceneViewsGeneratorHost
{
    private SceneViewsGenerator<ObjectsGroupView>? _generator;

    public ObjectsGroupView()
    {
        Loaded += OnLoadedFirstTime;
        Loaded += OnLoaded;
    }

    private IEnumerable<SceneObjectView> ItemsInternal => _generator is null ? Items : _generator.Views;

    #region ItemsSource

    public IEnumerable<object>? ItemsSource
    {
        get => (IEnumerable<object>?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable<object>), typeof(ObjectsGroupView), new PropertyMetadata(default(IEnumerable<object>?), OnItemsSourceChanged));

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ObjectsGroupView groupView) return;

        if (!groupView.IsLoaded) return;

        groupView.UpdateGenerator();
    }

    #endregion

    #region ItemTemplateSelector

    public DataTemplateSelector ItemTemplateSelector
    {
        get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
        set => SetValue(ItemTemplateSelectorProperty, value);
    }

    public static readonly DependencyProperty ItemTemplateSelectorProperty =
        DependencyProperty.Register(nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(ObjectsGroupView), new PropertyMetadata(default(DataTemplateSelector), OnItemTemplateSelectorChanged));

    private static void OnItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ObjectsGroupView groupView) return;

        if (!groupView.IsLoaded) return;

        groupView.UpdateGenerator();
    }

    #endregion

    public List<SceneObjectView> Items { get; } = [];

    public override bool HitTest(Vector2 pos, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var totalMatrix = TransformMatrix * worldTransform;
        return ItemsInternal.Any(x => x.HitTest(pos, totalMatrix, tolerance));
    }

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var totalMatrix = TransformMatrix * worldTransform;
        var res = ItemsInternal.Select(x => x.HitTest(inputGeometry, totalMatrix, tolerance)).ToArray();

        if (res.All(x => x is GeometryRelation.IsContained))
            return GeometryRelation.IsContained;

        if (res.All(x => x is GeometryRelation.Disjoint))
            return GeometryRelation.Disjoint;

        if (res.Any(x => x is GeometryRelation.Overlap or GeometryRelation.IsContained))
            return GeometryRelation.Overlap;

        if (res.Any(x => x is GeometryRelation.Contains))
            return GeometryRelation.Contains;

        return GeometryRelation.Unknown;
    }

    protected override IEnumerator LogicalChildren => ItemsInternal.GetEnumerator();

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        if (!IsLoaded)
            return;

        if (!ItemsInternal.Any(x => x.IsLoaded))
            return;

        foreach (var item in ItemsInternal)
        {
            item.Render(scene, context);
        }
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        if (!IsLoaded)
            return;

        if (!ItemsInternal.Any(x => x.IsLoaded))
            return;

        foreach (var item in ItemsInternal)
        {
            item.RenderSelection(scene, context);
        }
    }

    protected override void OnCacheChanged(ResourceCache cache)
    {
        if (_generator is not null)
        {
            _generator.UpdateCache(cache);
        }
        else
        {
            foreach (var item in Items)
            {
                item.InitializeCache(cache);
            }
        }

        base.OnCacheChanged(cache);
    }

    protected override bool OnIsDirtyEvaluation() => ItemsInternal.Any(x => x.IsDirty);

    private void UpdateGenerator()
    {
        if (ItemsSource is null)
            return;

        if (_generator is not null)
        {
            _generator.ItemsCollectionChanged -= OnGeneratorItemsCollectionChanged;
            _generator.Dispose();
        }

        _generator = new SceneViewsGenerator<ObjectsGroupView>(this, ItemTemplateSelector, ItemsSource, Cache);
        _generator.ItemsCollectionChanged += OnGeneratorItemsCollectionChanged;
    }

    private void OnGeneratorItemsCollectionChanged() => MakeDirty();

    private void OnLoadedFirstTime(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoadedFirstTime;

        if (ItemsSource is not null)
        {
            UpdateGenerator();
        }
        else
        {
            foreach (var item in Items)
            {
                item.DataContext = DataContext;
                AddLogicalChild(item);
            }
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => MakeDirty();

    protected override void Dispose(bool disposingManaged)
    {
        if (_generator is not null)
        {
            _generator.Dispose();
            _generator.ItemsCollectionChanged -= OnGeneratorItemsCollectionChanged;
        }

        foreach (var item in Items)
        {
            RemoveLogicalChild(item);
            item.Dispose();
        }

        Items.Clear();

        base.Dispose(disposingManaged);
    }

    void ISceneViewsGeneratorHost.AddLogicalChild(object child) => AddLogicalChild(child);

    void ISceneViewsGeneratorHost.RemoveLogicalChild(object child) => RemoveLogicalChild(child);
}
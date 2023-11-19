using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Base;
using LogicSimulator.Utils;
using SharpDX;
using Point = System.Windows.Point;

namespace LogicSimulator.Scene;

[ContentProperty(nameof(Layers))]
public class Scene2D : FrameworkElement, IDisposable
{
    private bool _isRenderRequested;

    private Matrix3x2 _scaleMatrix = Matrix3x2.Identity;
    private Matrix3x2 _translationMatrix = Matrix3x2.Identity;
    private Matrix3x2 _rotationMatrix = Matrix3x2.Identity;

    private SceneRenderer? _renderer;

    private D2DContext Context => _renderer?.D2DContext ?? throw new ApplicationException("Context is not initialized.");

    #region Scale

    public float Scale
    {
        get => (float)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    public static readonly DependencyProperty ScaleProperty =
        DependencyProperty.Register(nameof(Scale), typeof(float), typeof(Scene2D), new PropertyMetadata(1f, OnScaleChanged));

    private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene) return;

        scene._scaleMatrix = Matrix3x2.Scaling((float)e.NewValue);

        scene._isRenderRequested = true;
    }

    #endregion

    #region Translation

    public Vector2 Translation
    {
        get => (Vector2)GetValue(TranslationProperty);
        set => SetValue(TranslationProperty, value);
    }

    public static readonly DependencyProperty TranslationProperty =
        DependencyProperty.Register(nameof(Translation), typeof(Vector2), typeof(Scene2D), new PropertyMetadata(Vector2.Zero, OnTranslationChanged));

    private static void OnTranslationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene) return;

        scene._translationMatrix = Matrix3x2.Translation((Vector2)e.NewValue);

        scene._isRenderRequested = true;
    }

    #endregion

    #region Rotation

    public float Rotation
    {
        get => (float)GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    public static readonly DependencyProperty RotationProperty =
        DependencyProperty.Register(nameof(Rotation), typeof(float), typeof(Scene2D), new PropertyMetadata(0f, OnRotationChanged));

    private static void OnRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene) return;

        scene._rotationMatrix = Matrix3x2.Rotation(MathUtil.DegreesToRadians((float)e.NewValue));

        scene._isRenderRequested = true;
    }

    #endregion

    #region MousePosition

    private static readonly DependencyPropertyKey MousePositionPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(MousePosition), typeof(Vector2), typeof(Scene2D), new PropertyMetadata(Vector2.Zero));

    public static readonly DependencyProperty MousePositionProperty
        = MousePositionPropertyKey.DependencyProperty;

    public Vector2 MousePosition
    {
        get => (Vector2)GetValue(MousePositionProperty);
        private set => SetValue(MousePositionPropertyKey, value);
    }

    #endregion

    #region IsRequiredRenderingEnabled

    public bool IsRequiredRenderingEnabled
    {
        get => (bool)GetValue(IsRequiredRenderingEnabledProperty);
        set => SetValue(IsRequiredRenderingEnabledProperty, value);
    }

    public static readonly DependencyProperty IsRequiredRenderingEnabledProperty =
        DependencyProperty.Register(nameof(IsRequiredRenderingEnabled), typeof(bool), typeof(Scene2D), new PropertyMetadata(true));

    #endregion

    public ObservableCollection<BaseSceneLayer> Layers { get; } = new();

    public Matrix3x2 Transform => _scaleMatrix * _rotationMatrix * _translationMatrix;

    public Size2F PixelSize => Context.DrawingContext.DrawingSize;

    //TODO: Оптимизировать (кешировать)
    public float Dpi => (float)VisualTreeHelper.GetDpi(this).PixelsPerInchX;

    public Scene2D()
    {
        ClipToBounds = true;
        Focusable = true;
        UseLayoutRounding = true;
        SnapsToDevicePixels = true;
        VisualEdgeMode = EdgeMode.Aliased;

        Layers.CollectionChanged += OnLayersCollectionChanged;
        Loaded += OnLoaded;
    }

    public Vector2 PointFromControlToSceneSpace(Point pos) => pos.ToVector2().DpiCorrect(Dpi).InvertAndTransform(Transform);

    public Vector2 PointFromControlToSceneSpace(Vector2 pos) => pos.DpiCorrect(Dpi).InvertAndTransform(Transform);

    protected override IEnumerator LogicalChildren => Layers.GetEnumerator();

    protected override void OnRender(DrawingContext drawingContext) => _renderer?.WpfRender(drawingContext, RenderSize);

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        if (_renderer is null)
            return;

        _renderer.Resize(sizeInfo.NewSize, Dpi, renderAfterResize: false);

        foreach (var layer in Layers)
        {
            layer.InitializeCache(Context.Cache);
        }

        _renderer.RequestRender();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        MousePosition = e.GetPosition(this).ToVector2().DpiCorrect(Dpi).InvertAndTransform(Transform);
    }

    private void OnLayersCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        switch (args.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddLogicalChild(args.NewItems![0]);
                break;
            case NotifyCollectionChangedAction.Remove:
                RemoveLogicalChild(args.NewItems![0]);
                break;
            case NotifyCollectionChangedAction.Replace:
                throw new NotImplementedException();
            case NotifyCollectionChangedAction.Move:
                throw new NotImplementedException();
            case NotifyCollectionChangedAction.Reset:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        if (_renderer is not null)
            return;

        var rootWindow = Window.GetWindow(this);

        if (rootWindow is null)
            throw new ApplicationException("Can't find scene root window.");

        var handle = new WindowInteropHelper(rootWindow).Handle;

        _renderer = new SceneRenderer(OnRender, handle);
        _renderer.Resize(RenderSize, Dpi, renderAfterResize: false);

        foreach (var layer in Layers)
        {
            layer.InitializeCache(Context.Cache);
        }

        InvalidateVisual();

        CompositionTarget.Rendering += OnCompositionTargetRendering;

        _renderer.RequestRender();
    }

    private void OnCompositionTargetRendering(object? sender, EventArgs e)
    {
        if (!IsVisible)
            return;

        if (IsRequiredRenderingEnabled)
        {
            if (_isRenderRequested || Layers.Any(x => x.IsDirty))
            {
                _renderer!.RequestRender();
                _isRenderRequested = false;
            }
        }
        else
        {
            _renderer!.RequestRender();
        }
    }

    private void OnRender(DirectXContext context)
    {
        RenderDebugger.BeginRender(this, Context);

        Context.DrawingContext.BeginDraw();
        Context.DrawingContext.PushTransform(Transform);

        foreach (var layer in Layers)
        {
            layer.Render(this, Context);
        }

        Context.DrawingContext.PopTransform();

        RenderDebugger.EndRender();
        RenderDebugger.DrawStatistics(this, Context, new Vector2(5));

        Context.DrawingContext.EndDraw();
    }

    public void Dispose()
    {
        CompositionTarget.Rendering -= OnCompositionTargetRendering;
        Layers.CollectionChanged -= OnLayersCollectionChanged;
        Loaded -= OnLoaded;

        Utilities.Dispose(ref _renderer);

        foreach (var layer in Layers)
        {
            layer.Dispose();
        }

        Layers.Clear();

        GC.Collect();
        GC.SuppressFinalize(this);
    }
}
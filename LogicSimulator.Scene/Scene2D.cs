using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Layers.Base;
using SharpDX;
using SharpDX.Direct2D1;
using Color = SharpDX.Color;
using FontStretch = SharpDX.DirectWrite.FontStretch;
using FontStyle = SharpDX.DirectWrite.FontStyle;
using FontWeight = SharpDX.DirectWrite.FontWeight;

namespace LogicSimulator.Scene;

[ContentProperty(nameof(Layers))]
public class Scene2D : FrameworkElement, IDisposable
{
    private bool _isRenderRequested;

    private SceneRenderer? _renderer;

    private D2DContext Context => _renderer?.D2DContext ?? throw new ApplicationException("Context is not initialized.");

    public ObservableCollection<BaseSceneLayer> Layers { get; } = new();

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

        scene._isRenderRequested = true;
    }

    #endregion

    public Matrix3x2 Transform => Context.DrawingContext.Transform;

    public Size2F PixelSize => Context.DrawingContext.DrawingSize;

    public float Dpi => (float)VisualTreeHelper.GetDpi(this).PixelsPerInchX;

    #region IsRequiredRenderingEnabled

    public bool IsRequiredRenderingEnabled
    {
        get => (bool)GetValue(IsRequiredRenderingEnabledProperty);
        set => SetValue(IsRequiredRenderingEnabledProperty, value);
    }

    public static readonly DependencyProperty IsRequiredRenderingEnabledProperty =
        DependencyProperty.Register(nameof(IsRequiredRenderingEnabled), typeof(bool), typeof(Scene2D), new PropertyMetadata(true));

    #endregion

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

    protected override IEnumerator LogicalChildren => Layers.GetEnumerator();

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
        Context.DrawingContext.BeginDraw();
        Context.DrawingContext.PushTransform(Matrix3x2.Scaling(Scale) * Matrix3x2.Rotation(MathUtil.DegreesToRadians(Rotation)) * Matrix3x2.Translation(Translation));

        foreach (var layer in Layers)
        {
            layer.Render(this, Context);
        }

        Context.DrawingContext.PopTransform();
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
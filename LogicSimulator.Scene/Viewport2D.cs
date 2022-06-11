using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using SharpDX;

namespace LogicSimulator.Scene;

public class Viewport2D : FrameworkElement
{
    #region Private fields

    private Renderer2D _renderer;
    private Dx11ImageSource _imageSource;

    private bool _isRendering;

    #endregion

    static Viewport2D()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Viewport2D), new FrameworkPropertyMetadata(typeof(Viewport2D)));
    }

    public Viewport2D()
    {
        SnapsToDevicePixels = true;
        UseLayoutRounding = true;
        ClipToBounds = true;

        _imageSource = new Dx11ImageSource();
        _renderer = new Renderer2D(_imageSource);

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    #region Scene

    public BaseScene2D Scene
    {
        get => (BaseScene2D)GetValue(SceneProperty);
        set => SetValue(SceneProperty, value);
    }

    public static readonly DependencyProperty SceneProperty =
        DependencyProperty.Register(nameof(Scene), typeof(BaseScene2D), typeof(Viewport2D), new FrameworkPropertyMetadata(default(BaseScene2D), FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region Private properties

    private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(this);

    #endregion

    protected override void OnRender(DrawingContext drawingContext)
    {
        drawingContext.DrawImage(_imageSource, new Rect(RenderSize));
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        if (_renderer is null) return;

        var (width, height) = GetDpiScaledSize();

        _renderer.Resize(width, height, _imageSource);

        base.OnRenderSizeChanged(sizeInfo);
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        //TODO: Костыль!
        Window.GetWindow(this)!.Closing += (_, _) =>
        {
            Utilities.Dispose(ref _imageSource);
            Utilities.Dispose(ref _renderer);
        };
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!IsVisible || Scene is null) return;

        StartRenderScene();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        StopRenderScene();
    }

    private void StartRenderScene()
    {
        if (IsInDesignMode || _isRendering) return;

        _imageSource.IsFrontBufferAvailableChanged += OnFrontBufferAvailableChanged;
        CompositionTarget.Rendering += OnRenderScene;
        _isRendering = true;
    }

    private void StopRenderScene()
    {
        if (IsInDesignMode || !_isRendering) return;

        _imageSource.IsFrontBufferAvailableChanged -= OnFrontBufferAvailableChanged;
        CompositionTarget.Rendering -= OnRenderScene;

        _isRendering = false;
    }

    private void OnRenderScene(object sender, EventArgs e)
    {
        _renderer.RenderScene(Scene);
        _imageSource.InvalidateD3DImage();
    }

    private void OnFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (_imageSource.IsFrontBufferAvailable) StartRenderScene();
        else StopRenderScene();
    }

    private (int width, int height) GetDpiScaledSize()
    {
        var dpi = VisualTreeHelper.GetDpi(this).DpiScaleX;
        var width = Math.Max((int)(ActualWidth * dpi), 10);
        var height = Math.Max((int)(ActualHeight * dpi), 10);

        return (width, height);
    }
}
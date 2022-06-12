using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.Direct2D1.Factory;

namespace LogicSimulator.Scene;

public class Scene2D : FrameworkElement
{
    private Dx11ImageSource _imageSource;

    private Device _device;
    private Factory _factory;
    private RenderTarget _renderTarget;
    private Texture2D _texture2D;
    private Surface _surface;

    private RenderTargetProperties _renderTargetProperties;
    private Texture2DDescription _texture2DDescription;

    private bool _isRendering;

    private Window _rootWindow;
    private bool _isRootWindowCloseEventHandled;

    private ObjectRenderer _objectRenderer;

    static Scene2D()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Scene2D), new FrameworkPropertyMetadata(typeof(Scene2D)));
    }

    public Scene2D()
    {
        SnapsToDevicePixels = true;
        UseLayoutRounding = true;
        ClipToBounds = true;

        _imageSource = new Dx11ImageSource();

        Init();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(this);

    private Window RootWindow => _rootWindow ??= Window.GetWindow(this);

    #region Objects

    public IEnumerable<BaseSceneObject> Objects
    {
        get => (IEnumerable<BaseSceneObject>)GetValue(ObjectsProperty);
        set => SetValue(ObjectsProperty, value);
    }

    public static readonly DependencyProperty ObjectsProperty =
        DependencyProperty.Register(nameof(Objects), typeof(IEnumerable<BaseSceneObject>), typeof(Scene2D), new PropertyMetadata(default(IEnumerable<BaseSceneObject>)));

    #endregion

    protected override void OnRender(DrawingContext drawingContext)
    {
        drawingContext.DrawImage(_imageSource, new Rect(RenderSize));
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        Resize();

        base.OnRenderSizeChanged(sizeInfo);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!_isRootWindowCloseEventHandled && !IsInDesignMode)
        {
            RootWindow.Closed += OnRootWindowClosing;
            _isRootWindowCloseEventHandled = true;
        }

        if (!IsVisible) return;

        StartRenderScene();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        StopRenderScene();
    }

    private void OnRootWindowClosing(object sender, EventArgs e)
    {
        Utilities.Dispose(ref _imageSource);
        Utilities.Dispose(ref _device);
        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _texture2D);
        Utilities.Dispose(ref _surface);
        Utilities.Dispose(ref _objectRenderer);

        foreach (var sceneObject in Objects)
        {
            sceneObject.Dispose();
        }
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
        _renderTarget.BeginDraw();

        _renderTarget.Clear(new Color4(0.7f, 0.7f, 0.7f, 1));

        foreach (var sceneObject in Objects)
        {
            sceneObject.Render(_objectRenderer);
        }

        _renderTarget.EndDraw();
        _device.ImmediateContext.Flush();
        _imageSource.InvalidateD3DImage();
    }

    private void OnFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (_imageSource.IsFrontBufferAvailable) StartRenderScene();
        else StopRenderScene();
    }

    private void Init()
    {
        _device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);
        _factory = new Factory();

        var (width, height) = GetDpiScaledSize();

        _texture2DDescription = new Texture2DDescription
        {
            BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
            Format = Format.B8G8R8A8_UNorm,
            Width = width,
            Height = height,
            MipLevels = 1,
            SampleDescription = new SampleDescription(1, 0),
            Usage = ResourceUsage.Default,
            OptionFlags = ResourceOptionFlags.Shared,
            CpuAccessFlags = CpuAccessFlags.None,
            ArraySize = 1
        };

        _texture2D = new Texture2D(_device, _texture2DDescription);

        _surface = _texture2D.QueryInterface<Surface>();

        _renderTargetProperties = new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, AlphaMode.Premultiplied));

        _renderTarget = new RenderTarget(_factory, _surface, _renderTargetProperties)
        {
            AntialiasMode = AntialiasMode.Aliased
        };

        _objectRenderer = new ObjectRenderer(_renderTarget);

        _device.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height);

        _imageSource.SetRenderTarget(_texture2D);
    }

    private void Resize()
    {
        var (width, height) = GetDpiScaledSize();

        _texture2DDescription.Width = width;
        _texture2DDescription.Height = height;

        var lastAntialiasMode = _renderTarget.AntialiasMode;

        Utilities.Dispose(ref _texture2D);
        Utilities.Dispose(ref _surface);
        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _objectRenderer);

        _texture2D = new Texture2D(_device, _texture2DDescription);

        _surface = _texture2D.QueryInterface<Surface>();

        _renderTarget = new RenderTarget(_factory, _surface, _renderTargetProperties)
        {
            AntialiasMode = lastAntialiasMode
        };

        _objectRenderer = new ObjectRenderer(_renderTarget);

        _device.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height);

        _imageSource.SetRenderTarget(_texture2D);
    }

    private (int width, int height) GetDpiScaledSize()
    {
        var dpi = VisualTreeHelper.GetDpi(this).DpiScaleX;
        var width = Math.Max((int)(ActualWidth * dpi), 10);
        var height = Math.Max((int)(ActualHeight * dpi), 10);

        return (width, height);
    }
}
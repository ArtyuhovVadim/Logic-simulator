using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.Direct2D1.Factory;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;

namespace LogicSimulator.Scene;

public class Scene2D : FrameworkElement
{
    private Device _device;
    private Texture2D _texture2D;
    private Dx11ImageSource _imageSource;
    private RenderTarget _renderTarget;
    private Factory _factory;

    private bool _isRendering;

    public Scene2D()
    {
        ClipToBounds = true;
        VisualEdgeMode = EdgeMode.Aliased;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(this);

    protected override void OnRender(DrawingContext drawingContext)
    {
        drawingContext.DrawImage(_imageSource, new Rect(RenderSize));
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        CreateAndBindTargets();
        base.OnRenderSizeChanged(sizeInfo);
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        StartDirect3D();

        Window.GetWindow(this)!.Closed += (_, _) => StopDirect3D();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (IsInDesignMode || !IsVisible) return;

        StartRendering();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (IsInDesignMode) return;

        StopRendering();
    }

    private void OnRendering(object sender, EventArgs e)
    {
        if (!_isRendering) return;

        _renderTarget.BeginDraw();

        _renderTarget.Clear(new RawColor4(0.5f, 0.5f, 0.5f, 1));

        using var brush = new SolidColorBrush(_renderTarget, new Color4(1, 0, 0, 1));

        _renderTarget.DrawRectangle(new RawRectangleF(100, 100, 300, 400), brush, 1);

        _renderTarget.EndDraw();
        _device.ImmediateContext.Flush();
        _imageSource.InvalidateD3DImage();
    }

    private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (_imageSource.IsFrontBufferAvailable) StartRendering();
        else StopRendering();
    }

    private void StartDirect3D()
    {
        _device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);

        _imageSource = new Dx11ImageSource();
        _imageSource.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

        CreateAndBindTargets();
    }

    private void StopDirect3D()
    {
        _imageSource.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;

        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _imageSource);
        Utilities.Dispose(ref _texture2D);
        Utilities.Dispose(ref _device);
    }

    private void CreateAndBindTargets()
    {
        _imageSource.SetRenderTarget(null);

        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _texture2D);

        var (width, height) = GetDpiScaledSize();

        var textureDescription = new Texture2DDescription
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

        _factory = new Factory();

        _texture2D = new Texture2D(_device, textureDescription);

        var surface = _texture2D.QueryInterface<Surface>();

        var renderTargetProperties = new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied));

        _renderTarget = new RenderTarget(_factory, surface, renderTargetProperties)
        {
            AntialiasMode = AntialiasMode.Aliased
        };

        _imageSource.SetRenderTarget(_texture2D);

        _device.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height);
    }

    private void StartRendering()
    {
        if (_isRendering) return;

        CompositionTarget.Rendering += OnRendering;
        _isRendering = true;
    }

    private void StopRendering()
    {
        if (!_isRendering) return;

        CompositionTarget.Rendering -= OnRendering;
        _isRendering = false;
    }

    private (int width, int height) GetDpiScaledSize()
    {
        var dpi = VisualTreeHelper.GetDpi(this).DpiScaleX;
        var width = Math.Max((int)(ActualWidth * dpi), 10);
        var height = Math.Max((int)(ActualHeight * dpi), 10);

        return (width, height);
    }
}
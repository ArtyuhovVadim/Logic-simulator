using System;
using System.Windows;
using System.Windows.Media;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.Direct2D1.Factory;
using GradientStop = SharpDX.Direct2D1.GradientStop;
using GradientStopCollection = SharpDX.Direct2D1.GradientStopCollection;
using LinearGradientBrush = SharpDX.Direct2D1.LinearGradientBrush;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace LogicSimulator.Scene;

public class SceneRenderer : IDisposable
{
    private Device _device;
    private Texture2D _texture2D;
    private Factory _factory;
    private RenderTarget _renderTarget;
    
    private Dx11ImageSource _imageSource;

    private Renderer _renderer;

    private GradientStopCollection _clearGradientStopCollection;
    private LinearGradientBrush _clearGradientBrush;

    private readonly Color4 _startClearColor = new(0.755f, 0.755f, 0.755f, 1f);
    private readonly Color4 _endClearColor = new(0.887f, 0.887f, 0.887f, 1f);

    public SceneRenderer(double pixelWidth, double pixelHeight, float dpi) => StartDirect3D(pixelWidth, pixelHeight, dpi);

    public RenderTarget RenderTarget
    {
        get => _renderTarget;
        private set => _renderTarget = value;
    }

    public bool IsRendering { get; set; } = true;

    public Matrix3x2 Transform
    {
        get => RenderTarget.Transform;
        set
        {
            RenderTarget.Transform = value;
            ResourceDependentObject.RequireRender();
        }
    }

    public void Dispose() => StopDirect3D();

    public void Render(Scene2D scene)
    {
        if (!IsRendering || !ResourceDependentObject.IsRequireRender) return;

        RenderTarget.BeginDraw();

        GradientClear();

        foreach (var component in scene.Components)
        {
            component.Render(scene, _renderer);
        }

        RenderTarget.EndDraw();
        _device.ImmediateContext.Flush();
        _imageSource.InvalidateD3DImage();

        ResourceDependentObject.EndRender();
    }

    public void WpfRender(DrawingContext drawingContext, Size renderSize)
    {
        drawingContext.DrawImage(_imageSource, new Rect(renderSize));
    }

    public void Resize(double pixelWidth, double pixelHeight, float dpi)
    {
        CreateAndBindTargets(pixelWidth, pixelHeight, dpi);

        ResourceDependentObject.RequireUpdateInAllResourceDependentObjects();
    }

    private void StartDirect3D(double pixelWidth, double pixelHeight, float dpi)
    {
        _device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);

        _imageSource = new Dx11ImageSource();
        _imageSource.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

        CreateAndBindTargets(pixelWidth, pixelHeight, dpi);
    }

    private void StopDirect3D()
    {
        _imageSource.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;

        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _imageSource);
        Utilities.Dispose(ref _texture2D);
        Utilities.Dispose(ref _device);
        Utilities.Dispose(ref _renderer);
    }

    private void CreateAndBindTargets(double pixelWidth, double pixelHeight, float dpi)
    {
        var transform = Matrix3x2.Identity;

        if (RenderTarget is not null) transform = RenderTarget.Transform;
        
        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _texture2D);
        Utilities.Dispose(ref _renderer);

        _imageSource.SetRenderTarget(null);

        var (width, height) = ((int) Math.Max(pixelWidth * dpi / 96f, 10), (int) Math.Max(pixelHeight * dpi / 96f, 10));

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

        RenderTarget = new RenderTarget(_factory, surface, renderTargetProperties)
        {
            AntialiasMode = AntialiasMode.Aliased,
            Transform = transform
        };

        CreateClearResources(_startClearColor, _endClearColor);

        _renderer = new Renderer(RenderTarget);

        _imageSource.SetRenderTarget(_texture2D);

        _device.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height);
    }

    private void GradientClear()
    {
        var tempTransform = Transform;
        Transform = Matrix3x2.Identity;

        RenderTarget.FillRectangle(new RectangleF(0, 0, RenderTarget.Size.Width, RenderTarget.Size.Height), _clearGradientBrush);

        Transform = tempTransform;
    }

    private void CreateClearResources(Color4 startColor, Color4 endColor)
    {
        Utilities.Dispose(ref _clearGradientBrush);
        Utilities.Dispose(ref _clearGradientStopCollection);

        _clearGradientStopCollection = new GradientStopCollection(RenderTarget, new GradientStop[]
        {
            new() {Position = 0f, Color = startColor},
            new() {Position = 1f, Color = endColor}
        });

        var properties = new LinearGradientBrushProperties
        {
            StartPoint = new Vector2(RenderTarget.Size.Width / 2, 0),
            EndPoint = new Vector2(RenderTarget.Size.Width / 2, RenderTarget.Size.Height)
        };

        _clearGradientBrush = new LinearGradientBrush(RenderTarget, properties, _clearGradientStopCollection);
    }

    private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e) => 
        IsRendering = _imageSource.IsFrontBufferAvailable;
}
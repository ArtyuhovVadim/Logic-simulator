using System;
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

namespace LogicSimulator.Scene;

public class SceneRenderer : IDisposable
{
    private Device _device;
    private Texture2D _texture2D;
    private RenderTarget _renderTarget;
    private Factory _factory;

    private Dx11ImageSource _imageSource;

    private ObjectRenderer _objectRenderer;
    private ComponentRenderer _componentRenderer;

    public SceneRenderer(int pixelWidth, int pixelHeight) => StartDirect3D(pixelWidth, pixelHeight);

    public bool IsRendering { get; set; } = true;

    public void Dispose() => StopDirect3D();

    public void Render(Scene2D scene)
    {
        if (!IsRendering) return;

        _renderTarget.BeginDraw();

        _renderTarget.Clear(new RawColor4(0.5f, 0.5f, 0.5f, 1));

        if (scene.RenderingComponents is not null)
        {
            foreach (var component in scene.RenderingComponents)
            {
                component.Render(_componentRenderer);
            }
        }

        if (scene.Objects is not null)
        {
            foreach (var sceneObject in scene.Objects)
            {
                sceneObject.Render(_objectRenderer);
            }
        }

        _renderTarget.EndDraw();
        _device.ImmediateContext.Flush();
        _imageSource.InvalidateD3DImage();
    }

    public void WpfRender(DrawingContext drawingContext, Size renderSize)
    {
        drawingContext.DrawImage(_imageSource, new Rect(renderSize));
    }

    public void Resize(int pixelWidth, int pixelHeight)
    {
        CreateAndBindTargets(pixelWidth, pixelHeight);
        //TODO: Возможно стоит перенести вызов в другое место
        ResourceDependentObject.RequireUpdateInAllResourceDependentObjects();
    }

    private void StartDirect3D(int pixelWidth, int pixelHeight)
    {
        _device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);

        _imageSource = new Dx11ImageSource();
        _imageSource.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

        CreateAndBindTargets(pixelWidth, pixelHeight);
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

    private void CreateAndBindTargets(int pixelWidth, int pixelHeight)
    {
        _imageSource.SetRenderTarget(null);

        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _texture2D);

        var textureDescription = new Texture2DDescription
        {
            BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
            Format = Format.B8G8R8A8_UNorm,
            Width = pixelWidth,
            Height = pixelHeight,
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

        _objectRenderer = new ObjectRenderer(_renderTarget);
        _componentRenderer = new ComponentRenderer(_renderTarget);

        _imageSource.SetRenderTarget(_texture2D);

        _device.ImmediateContext.Rasterizer.SetViewport(0, 0, pixelWidth, pixelHeight);
    }

    private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e) =>
        IsRendering = _imageSource.IsFrontBufferAvailable;
}
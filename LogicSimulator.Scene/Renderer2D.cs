using System;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.Direct2D1.Factory;

namespace LogicSimulator.Scene;

public class Renderer2D : IDisposable
{
    #region Private fields

    private Device _device;
    private Factory _factory;
    private RenderTarget _renderTarget;
    private Texture2D _texture2D;
    private Surface _surface;

    private readonly RenderTargetProperties _renderTargetProperties;
    private Texture2DDescription _texture2DDescription;

    #endregion

    public Renderer2D(Dx11ImageSource imageSource, AntialiasMode antialiasMode = AntialiasMode.Aliased)
        : this(1, 1, imageSource, antialiasMode)
    { }

    public Renderer2D(int pixelWidth, int pixelHeight, Dx11ImageSource imageSource, AntialiasMode antialiasMode = AntialiasMode.Aliased)
    {
        PixelWidth = pixelWidth;
        PixelHeight = pixelHeight;

        _device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);
        _factory = new Factory();

        _texture2DDescription = new Texture2DDescription
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

        _texture2D = new Texture2D(_device, _texture2DDescription);

        _surface = _texture2D.QueryInterface<Surface>();

        _renderTargetProperties = new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied));

        _renderTarget = new RenderTarget(_factory, _surface, _renderTargetProperties)
        {
            AntialiasMode = antialiasMode
        };

        _device.ImmediateContext.Rasterizer.SetViewport(0, 0, pixelWidth, pixelHeight);

        imageSource.SetRenderTarget(_texture2D);
    }

    #region Public properties

    public int PixelWidth { get; private set; }
    public int PixelHeight { get; private set; }

    #endregion

    public void Resize(int pixelWidth, int pixelHeight, Dx11ImageSource imageSource)
    {
        PixelWidth = pixelWidth;
        PixelHeight = pixelHeight;

        _texture2DDescription.Width = pixelWidth;
        _texture2DDescription.Height = pixelHeight;

        var lastAntialiasMode = _renderTarget.AntialiasMode;

        Utilities.Dispose(ref _texture2D);
        Utilities.Dispose(ref _surface);
        Utilities.Dispose(ref _renderTarget);

        _texture2D = new Texture2D(_device, _texture2DDescription);

        _surface = _texture2D.QueryInterface<Surface>();

        _renderTarget = new RenderTarget(_factory, _surface, _renderTargetProperties)
        {
            AntialiasMode = lastAntialiasMode
        };

        _device.ImmediateContext.Rasterizer.SetViewport(0, 0, pixelWidth, pixelHeight);

        imageSource.SetRenderTarget(_texture2D);
    }

    public void RenderScene(BaseScene2D scene)
    {
        if (scene is null) 
            throw new NullReferenceException(nameof(scene));

        _renderTarget.BeginDraw();

        scene.Render(_renderTarget);

        _renderTarget.EndDraw();
        _device.ImmediateContext.Flush();
    }

    public void Dispose()
    {
        Utilities.Dispose(ref _device);
        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _texture2D);
        Utilities.Dispose(ref _surface);
    }
}
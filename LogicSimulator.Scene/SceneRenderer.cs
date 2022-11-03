using Microsoft.Wpf.Interop.DirectX;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.WIC;
using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory1 = SharpDX.Direct2D1.Factory1;
using TextFactory = SharpDX.DirectWrite.Factory;

namespace LogicSimulator.Scene;

public class SceneRenderer : IDisposable
{
    private readonly Scene2D _scene;

    private readonly D3D11Image _imageSource;

    private RenderTarget _renderTarget;
    private Factory1 _factory;
    private TextFactory _textFactory;

    private SharpDX.WIC.Bitmap _wicBitmap;
    private ImagingFactory _wicFactory;
    private WicRenderTarget _wicRenderTarget;

    private bool _isInitialized;

    internal RenderTarget RenderTarget => _renderTarget;
    internal Factory1 Factory => _factory;
    internal TextFactory TextFactory => _textFactory;

    public Matrix3x2 Transform
    {
        get => _renderTarget.Transform;
        set => _renderTarget.Transform = value;
    }

    public Vector2 RenderSize => new(_imageSource.PixelWidth, _imageSource.PixelHeight);

    public SceneRenderer(Scene2D scene)
    {
        _scene = scene;

        _imageSource = new D3D11Image { OnRender = OnRender };
        _imageSource.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

        CompositionTarget.Rendering += (_, _) =>
        {
            if (RenderNotifier.IsRenderRequired(_scene))
                RequestRender();
        };
    }

    //TODO: Сделать заебись
    public void RenderToStream(Stream stream)
    {
        var tmp = _renderTarget;

        _wicRenderTarget.Transform = tmp.Transform;

        _renderTarget = _wicRenderTarget;
        ResourceCache.RequestUpdateInAllResources();
        RenderScene(_scene, _wicRenderTarget);

        _renderTarget = tmp;
        ResourceCache.RequestUpdateInAllResources();

        var wicStream = new WICStream(_wicFactory, stream);
        var encoder = new BitmapEncoder(_wicFactory, ContainerFormatGuids.Png);
        encoder.Initialize(wicStream);

        var bitmapFrameEncode = new BitmapFrameEncode(encoder);
        bitmapFrameEncode.Initialize();
        bitmapFrameEncode.SetSize(5000, 5000);
        var a = SharpDX.WIC.PixelFormat.FormatDontCare;
        bitmapFrameEncode.SetPixelFormat(ref a);
        bitmapFrameEncode.WriteSource(_wicBitmap);

        bitmapFrameEncode.Commit();
        encoder.Commit();

        bitmapFrameEncode.Dispose();
        encoder.Dispose();
        wicStream.Dispose();
    }

    public void RequestRender() => _imageSource.RequestRender();

    public void WpfRender(DrawingContext drawingContext) => drawingContext.DrawImage(_imageSource, new Rect(_scene.RenderSize));

    public void Resize(Size size) => SetImageSourcePixelSize(size);

    public void SetOwnerWindow()
    {
        var window = Window.GetWindow(_scene);

        if (window is null) return;

        var handle = new WindowInteropHelper(window).Handle;

        if (handle == _imageSource.WindowOwner) return;

        _imageSource.WindowOwner = handle;

        SetImageSourcePixelSize(_scene.RenderSize);

        RequestRender();
    }

    private void OnRender(IntPtr resourceHandle, bool isNewSurface)
    {
        if (!_isInitialized || isNewSurface)
        {
            InitializeResources(resourceHandle);
            ResourceCache.RequestUpdateInAllResources();
        }

        RenderScene(_scene, _renderTarget);
    }

    //TODO: Разобраться
    private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (!_imageSource.IsFrontBufferAvailable)
        {
            _isInitialized = false;
        }
        else
        {
            RequestRender();
        }
    }

    private void SetImageSourcePixelSize(Size size)
    {
        var (width, height) = ((int)Math.Max(Math.Round(size.Width * _scene.Dpi / 96f), 10), (int)Math.Max(Math.Round(size.Height * _scene.Dpi / 96f), 10));

        _imageSource.SetPixelSize(width, height);

        RequestRender();
    }

    private void InitializeResources(IntPtr resourceHandle)
    {
        var tempTransform = Matrix3x2.Identity;

        if (_renderTarget is not null)
            tempTransform = _renderTarget.Transform;

        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _textFactory);
        Utilities.Dispose(ref _renderTarget);

        Utilities.Dispose(ref _wicFactory);
        Utilities.Dispose(ref _wicBitmap);
        Utilities.Dispose(ref _wicRenderTarget);

        using var comObject = new ComObject(resourceHandle);
        using var resource = comObject.QueryInterface<SharpDX.DXGI.Resource>();
        using var texture = resource.QueryInterface<SharpDX.Direct3D11.Texture2D>();
        using var surface = texture.QueryInterface<SharpDX.DXGI.Surface1>();

        var pixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied);

        var properties = new RenderTargetProperties
        {
            DpiX = 96,
            DpiY = 96,
            MinLevel = FeatureLevel.Level_DEFAULT,
            PixelFormat = pixelFormat,
            Type = RenderTargetType.Default,
            Usage = RenderTargetUsage.None
        };

        _factory = new Factory1();
        _textFactory = new TextFactory();

        _renderTarget = new RenderTarget(_factory, surface, properties)
        {
            AntialiasMode = AntialiasMode.Aliased,
            Transform = tempTransform
        };

        _wicFactory = new ImagingFactory();
        //TODO: Разобраться с константным размером
        _wicBitmap = new SharpDX.WIC.Bitmap(_wicFactory, 3000, 3000, SharpDX.WIC.PixelFormat.Format32bppBGR, BitmapCreateCacheOption.CacheOnLoad);
        _wicRenderTarget = new WicRenderTarget(_factory, _wicBitmap, properties with { PixelFormat = pixelFormat with { Format = Format.Unknown, AlphaMode = AlphaMode.Unknown } })
        {
            AntialiasMode = AntialiasMode.Aliased
        };

        _isInitialized = true;
    }

    private void RenderScene(Scene2D scene, RenderTarget renderTarget)
    {
        renderTarget.BeginDraw();

        foreach (var component in scene.Components)
        {
            component.Render(scene, renderTarget);
        }

        renderTarget.EndDraw();

        RenderNotifier.RenderEnd(_scene);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing) return;

        _imageSource?.Dispose();
        _renderTarget?.Dispose();
        _factory?.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~SceneRenderer()
    {
        Dispose(false);
    }
}
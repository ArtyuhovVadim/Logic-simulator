using Microsoft.Wpf.Interop.DirectX;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.WIC;
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
            if (!_scene.IsRequiredRenderingEnabled || RenderNotifier.IsRenderRequired(_scene))
                RequestRender();
        };
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

    public void RenderToStream(Stream stream, int width, int height, bool isAliased = true, float scale = 1f, Vector2 translation = default)
    {
        if (width <= 0 || height <= 0)
        {
            throw new ArgumentException("Size can't be zero or less!");
        }

        var properties = new RenderTargetProperties
        {
            DpiX = 96,
            DpiY = 96,
            MinLevel = FeatureLevel.Level_DEFAULT,
            PixelFormat = _renderTarget.PixelFormat with { Format = Format.Unknown, AlphaMode = AlphaMode.Unknown },
            Type = RenderTargetType.Default,
            Usage = RenderTargetUsage.None
        };

        var wicFactory = new ImagingFactory();
        var wicBitmap = new SharpDX.WIC.Bitmap(wicFactory, width, height, SharpDX.WIC.PixelFormat.Format32bppBGR, BitmapCreateCacheOption.CacheOnLoad);
        var wicRenderTarget = new WicRenderTarget(_factory, wicBitmap, properties)
        {
            AntialiasMode = isAliased ? AntialiasMode.Aliased : AntialiasMode.PerPrimitive,
            Transform = new Matrix3x2 { ScaleVector = new Vector2(scale, scale), TranslationVector = translation }
        };

        var tmp = _renderTarget;
        _renderTarget = wicRenderTarget;

        ResourceCache.RequestUpdateInAllResources();
        RenderScene(_scene, wicRenderTarget);

        _renderTarget = tmp;
        ResourceCache.RequestUpdateInAllResources();

        var wicStream = new WICStream(wicFactory, stream);
        var encoder = new BitmapEncoder(wicFactory, ContainerFormatGuids.Png);
        var format = SharpDX.WIC.PixelFormat.FormatDontCare;

        encoder.Initialize(wicStream);

        var bitmapFrameEncode = new BitmapFrameEncode(encoder);

        bitmapFrameEncode.Initialize();
        bitmapFrameEncode.SetSize(width, height);
        bitmapFrameEncode.SetPixelFormat(ref format);
        bitmapFrameEncode.WriteSource(wicBitmap);
        bitmapFrameEncode.Commit();

        encoder.Commit();

        bitmapFrameEncode.Dispose();
        encoder.Dispose();
        wicStream.Dispose();

        Utilities.Dispose(ref wicFactory);
        Utilities.Dispose(ref wicBitmap);
        Utilities.Dispose(ref wicRenderTarget);
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
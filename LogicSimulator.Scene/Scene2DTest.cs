using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Wpf.Interop.DirectX;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;

namespace LogicSimulator.Scene;

public class Scene2DTest : FrameworkElement
{
    private readonly D3D11Image _imageSource;

    private Factory1 _factory;
    private RenderTarget _renderTarget;

    private bool _isInitialized;

    public Scene2DTest()
    {
        VisualEdgeMode = EdgeMode.Aliased;
        ClipToBounds = true;

        _imageSource = new D3D11Image { OnRender = OnRender };

        _imageSource.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

        Loaded += OnLoaded;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        drawingContext.DrawImage(_imageSource, new Rect(RenderSize));
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        SetImageSourcePixelSize(RenderSize);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);

        if (window is null) return;

        var handle = new WindowInteropHelper(window).Handle;

        if (handle == _imageSource.WindowOwner) return;

        _imageSource.WindowOwner = handle;

        SetImageSourcePixelSize(RenderSize);
    }

    private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (!_imageSource.IsFrontBufferAvailable)
        {
            _isInitialized = false;
        }
        else
        {
            _imageSource.RequestRender();
        }
    }

    private void OnRender(IntPtr resourceHandle, bool isNewSurface)
    {
        if (!_isInitialized || isNewSurface)
        {
            InitializeResources(resourceHandle);
        }

        _renderTarget.BeginDraw();

        _renderTarget.Clear(new RawColor4(0.6f, 0.6f, 0.6f, 1));

        using var brush = new SolidColorBrush(_renderTarget, new RawColor4(0, 0, 1, 1));

        var rand = new Random(228);

        for (var i = 0; i < 100; i++)
        {
            _renderTarget.DrawEllipse(
                new Ellipse(new Vector2(rand.NextFloat(0, 2000), rand.NextFloat(0, 1000)), 100, 100), brush, 1);
        }

        _renderTarget.EndDraw();
    }

    private void InitializeResources(IntPtr resourceHandle)
    {
        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _renderTarget);

        using var comObject = new ComObject(resourceHandle);
        using var resource = comObject.QueryInterface<SharpDX.DXGI.Resource>();
        using var texture = resource.QueryInterface<SharpDX.Direct3D11.Texture2D>();
        using var surface = texture.QueryInterface<SharpDX.DXGI.Surface1>();

        var properties = new RenderTargetProperties
        {
            DpiX = 96,
            DpiY = 96,
            MinLevel = FeatureLevel.Level_DEFAULT,
            PixelFormat =
                new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
            Type = RenderTargetType.Default,
            Usage = RenderTargetUsage.None
        };

        _factory = new Factory1();

        _renderTarget = new RenderTarget(_factory, surface, properties)
        {
            AntialiasMode = AntialiasMode.Aliased
        };

        _isInitialized = true;
    }

    private void SetImageSourcePixelSize(Size size)
    {
        var dpi = (float)VisualTreeHelper.GetDpi(this).PixelsPerInchX;

        var (width, height) = ((int)Math.Max(size.Width * dpi / 96f, 10), (int)Math.Max(size.Height * dpi / 96f, 10));

        _imageSource.SetPixelSize(width, height);
    }
}
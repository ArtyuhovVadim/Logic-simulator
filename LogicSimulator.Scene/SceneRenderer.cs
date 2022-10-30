using System;
using System.Windows;
using Microsoft.Wpf.Interop.DirectX;
using SharpDX.Direct2D1;
using SharpDX;
using System.Windows.Media;
using System.Windows.Interop;
using Factory1 = SharpDX.Direct2D1.Factory1;
using FontStretch = SharpDX.DirectWrite.FontStretch;
using FontStyle = SharpDX.DirectWrite.FontStyle;
using FontWeight = SharpDX.DirectWrite.FontWeight;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;
using TextFactory = SharpDX.DirectWrite.Factory;

namespace LogicSimulator.Scene;

public class SceneRenderer : IDisposable
{
    private readonly Scene2D _scene;

    private readonly D3D11Image _imageSource;

    private RenderTarget _renderTarget;
    private Factory1 _factory;
    private TextFactory _textFactory;

    private int _count;
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

        _renderTarget.BeginDraw();

        foreach (var component in _scene.Components)
        {
            component.Render(_scene, RenderTarget);
        }

        var tmp = _renderTarget.Transform;

        _renderTarget.Transform = Matrix3x2.Identity;

        using var brush = new SolidColorBrush(_renderTarget, Color4.Black);
        using var textFormat = _scene.ResourceFactory.CreateTextFormat("Calibri", FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, 24);
        using var textLayout = _scene.ResourceFactory.CreateTextLayout($"Render calls count: {_count++}", textFormat);
        
        _renderTarget.DrawTextLayout(new Vector2(10, 10), textLayout, brush, DrawTextOptions.None);

        _renderTarget.Transform = tmp;

        _renderTarget.EndDraw();

        RenderNotifier.RenderEnd(_scene);
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
            PixelFormat = new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
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
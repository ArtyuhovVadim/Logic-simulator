using System;
using System.Windows;
using Microsoft.Wpf.Interop.DirectX;
using SharpDX.Direct2D1;
using SharpDX;
using System.Windows.Media;
using System.Windows.Interop;
using SharpDX.Mathematics.Interop;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace LogicSimulator.Scene;

public class SceneRenderer : IDisposable
{
    private readonly Scene2D _scene;

    private readonly D3D11Image _imageSource;

    private RenderTarget _renderTarget;
    private Factory _factory;

    private bool _isInitialized;

    internal RenderTarget RenderTarget => _renderTarget;

    public Matrix3x2 Transform
    {
        get => _renderTarget.Transform;
        set => _renderTarget.Transform = value;
    }

    public SceneRenderer(Scene2D scene)
    {
        _scene = scene;

        _imageSource = new D3D11Image { OnRender = OnRender };
        _imageSource.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;
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

        _imageSource.RequestRender();
    }

    private void OnRender(IntPtr resourceHandle, bool isNewSurface)
    {
        if (!_isInitialized || isNewSurface)
        {
            InitializeResources(resourceHandle);
        }

        _renderTarget.BeginDraw();

        foreach (var component in _scene.Components)
        {
            component.Render(_scene, RenderTarget);
        }

        _renderTarget.EndDraw();
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
            _imageSource.RequestRender();
        }
    }

    private void SetImageSourcePixelSize(Size size)
    {
        var (width, height) = ((int)Math.Max(size.Width * _scene.Dpi / 96f, 10), (int)Math.Max(size.Height * _scene.Dpi / 96f, 10));

        _imageSource.SetPixelSize(width, height);
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

        _renderTarget = new RenderTarget(_factory, surface, properties)
        {
            AntialiasMode = AntialiasMode.Aliased,
            Transform = tempTransform
        };

        _isInitialized = true;

        ResourceCache.RequestUpdateInAllResources();
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
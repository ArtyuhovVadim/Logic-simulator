using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Wpf.Interop.DirectX;
using SharpDX;
using SharpDX.Direct2D1;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory1 = SharpDX.Direct2D1.Factory1;
using FeatureLevel = SharpDX.Direct2D1.FeatureLevel;
using Matrix = SharpDX.Matrix;

namespace LogicSimulator.Scene;

public class SceneRenderer : IDisposable
{
    private readonly Scene2D _scene;
    private D3D11Image _imageSource;

    private Factory1 _factory;
    private RenderTarget _renderTarget;

    private bool _isInitialized;

    private Renderer _renderer;

    public SceneRenderer(Scene2D scene)
    {
        _scene = scene;

        var window = Window.GetWindow(scene);

        if (window is null)
            throw new ApplicationException("Not find root window for Scene2D!");

        var handle = new WindowInteropHelper(window).Handle;

        _imageSource = new D3D11Image
        {
            OnRender = OnRender,
            WindowOwner = handle
        };

        SetImageSourcePixelSize(scene.RenderSize);

        _imageSource.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;
    }

    public RenderTarget RenderTarget => _renderTarget;

    public Matrix3x2 Transform
    {
        get => RenderTarget.Transform;
        set
        {
            RenderTarget.Transform = value;
            RequestRender();
        }
    }

    public void Dispose()
    {
        Utilities.Dispose(ref _imageSource);
        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _renderer);
    }

    public void WpfRender(DrawingContext drawingContext, Size size)
    {
        drawingContext.DrawImage(_imageSource, new Rect(size));
    }

    public void Resize(Size size)
    {
        ResourceDependentObject.RequireUpdateInAllResourceDependentObjects();
        SetImageSourcePixelSize(size);
    }

    public void RequestRender()
    {
        ResourceDependentObject.RequireRender();
        _imageSource.RequestRender();
    }

    private void OnRender(IntPtr resourceHandle, bool isNewSurface)
    {
        if (!_isInitialized || isNewSurface)
        {
            InitializeResources(resourceHandle);
        }

        RenderTarget.BeginDraw();

        RenderTarget.Clear(new Color4(0.6f, 0.6f, 0.6f, 1));

        foreach (var component in _scene.Components)
        {
            component.Render(_scene, _renderer);
        }

        RenderTarget.EndDraw();

        ResourceDependentObject.EndRender();
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

    private void InitializeResources(IntPtr resourceHandle)
    {
        var tempTransform = Matrix3x2.Identity;

        if(_renderTarget is not null) 
            tempTransform = _renderTarget.Transform;

        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _renderer);

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
            AntialiasMode = AntialiasMode.Aliased,
            Transform = tempTransform
        };

        _renderer = new Renderer(_renderTarget);

        _isInitialized = true;
    }

    private void SetImageSourcePixelSize(Size size)
    {
        var dpi = _scene.Dpi;

        var (width, height) = ((int)Math.Max(size.Width * dpi / 96f, 10), (int)Math.Max(size.Height * dpi / 96f, 10));

        _imageSource.SetPixelSize(width, height);
    }
}
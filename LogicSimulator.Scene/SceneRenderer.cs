using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.Direct2D1.Factory;
using FontStretch = SharpDX.DirectWrite.FontStretch;
using FontStyle = SharpDX.DirectWrite.FontStyle;
using FontWeight = SharpDX.DirectWrite.FontWeight;
using GradientStop = SharpDX.Direct2D1.GradientStop;
using GradientStopCollection = SharpDX.Direct2D1.GradientStopCollection;
using LinearGradientBrush = SharpDX.Direct2D1.LinearGradientBrush;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using RectangleGeometry = SharpDX.Direct2D1.RectangleGeometry;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;

namespace LogicSimulator.Scene;

public class SceneRenderer : IDisposable
{
    private Device _device;
    private Texture2D _texture2D;
    private RenderTarget _renderTarget;
    private Factory _factory;

    private Dx11ImageSource _imageSource;

    private Renderer _renderer;

    private GradientStopCollection _clearGradientStopCollection;
    private LinearGradientBrush _clearGradientBrush;

    private readonly Color4 _startClearColor = new(0.755f, 0.755f, 0.755f, 1f);
    private readonly Color4 _endClearColor = new(0.887f, 0.887f, 0.887f, 1f);

    public SceneRenderer(double pixelWidth, double pixelHeight, float dpi) => StartDirect3D(pixelWidth, pixelHeight, dpi);

    public bool IsRendering { get; set; } = true;

    public Matrix3x2 Transform
    {
        get => _renderTarget.Transform;
        set
        {
            _renderTarget.Transform = value;
            ResourceDependentObject.RequireRender();
        }
    }

    public void Dispose() => StopDirect3D();

    public void Render(Scene2D scene)
    {
        if (!IsRendering || !ResourceDependentObject.IsRequireRender) return;

        _renderTarget.BeginDraw();

        GradientClear();

        foreach (var component in scene.Components)
        {
            component.Render(_renderer);
        }

        foreach (var sceneObject in scene.Objects)
        {
            sceneObject.Render(_renderer);
        }

        foreach (var sceneObject in scene.Objects)
        {
            if (sceneObject.IsSelected)
            {
                sceneObject.RenderSelection(_renderer);
            }
        }

#if DEBUG
        DebugRender();
#endif

        _renderTarget.EndDraw();
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
    }

    private void CreateAndBindTargets(double pixelWidth, double pixelHeight, float dpi)
    {
        var transform = Matrix3x2.Identity;

        if (_renderTarget is not null) transform = _renderTarget.Transform;

        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _factory);
        Utilities.Dispose(ref _texture2D);
        Utilities.Dispose(ref _renderer);

        _imageSource.SetRenderTarget(null);

        var (width, height) = ((int)Math.Max(pixelWidth * dpi / 96f, 10), (int)Math.Max(pixelHeight * dpi / 96f, 10));

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
            AntialiasMode = AntialiasMode.Aliased,
            Transform = transform
        };

        CreateClearResources(_startClearColor, _endClearColor);

        _renderer = new Renderer(_renderTarget);

        _imageSource.SetRenderTarget(_texture2D);

        _device.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height);
    }

    private void GradientClear()
    {
        var tempTransform = Transform;
        Transform = Matrix3x2.Identity;

        _renderTarget.FillRectangle(new RectangleF(0, 0, _renderTarget.Size.Width, _renderTarget.Size.Height), _clearGradientBrush);

        Transform = tempTransform;
    }

    private void CreateClearResources(Color4 startColor, Color4 endColor)
    {
        Utilities.Dispose(ref _clearGradientBrush);
        Utilities.Dispose(ref _clearGradientStopCollection);

        _clearGradientStopCollection = new GradientStopCollection(_renderTarget, new GradientStop[]
        {
            new() { Position = 0f, Color = startColor },
            new() { Position = 1f, Color = endColor }
        });

        var properties = new LinearGradientBrushProperties
        {
            StartPoint = new Vector2(_renderTarget.Size.Width / 2, 0),
            EndPoint = new Vector2(_renderTarget.Size.Width / 2, _renderTarget.Size.Height)
        };

        _clearGradientBrush = new LinearGradientBrush(_renderTarget, properties, _clearGradientStopCollection);
    }

    private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e) =>
        IsRendering = _imageSource.IsFrontBufferAvailable;

    private Dictionary<object, Dictionary<int, object>> GetAllResources()
    {
        var dict = new Dictionary<object, Dictionary<int, object>>();

        var objects = typeof(ResourceDependentObject).GetField("AllResourceDependentObject", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null) as List<ResourceDependentObject>;

        foreach (var o in objects!)
        {
            var resources = typeof(ResourceDependentObject).GetField("_resources", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(o) as Dictionary<int, object>;

            dict.Add(o, resources);
        }

        return dict;
    }

    private object GetResValue(object res)
    {
        switch (res)
        {
            case SolidColorBrush brush:
                return (Color4)brush.Color;
            case RectangleGeometry rectangleGeometry:
                var rect = rectangleGeometry.Rectangle;
                return new RectangleF { Top = rect.Top, Right = rect.Right, Bottom = rect.Bottom, Left = rect.Left };
            default:
                return res;
        }
    }

    private string GetObjString(object obj)
    {
        var str = string.Empty;

        str += $"Type: {obj.GetType().Name}";

        switch (obj)
        {
            case BaseSceneObject baseSceneObject:
                str += $" - IsSelected: {baseSceneObject.IsSelected} - IsDragging: {baseSceneObject.IsDragging}";
                break;
            default:
                break;
        }

        return str;
    }

    private void DebugRender()
    {
        var tmp = _renderTarget.Transform;

        _renderTarget.Transform = Matrix3x2.Identity;

        using var factory = new SharpDX.DirectWrite.Factory();
        using var textFormat = new TextFormat(factory, "ISOCPEUR", FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, 15);
        using var brush = new SolidColorBrush(_renderTarget, Color4.Black);

        var counter = 1;

        foreach (var (obj, res) in GetAllResources())
        {
            _renderTarget.DrawText($"{GetObjString(obj)}", textFormat, new RectangleF(50, counter * 20, 1000, 1000), brush, DrawTextOptions.None);

            counter++;

            foreach (var (hash, resVal) in res)
            {
                _renderTarget.DrawText($"Hash: {hash} - Type: {resVal.GetType().Name} - Value: {GetResValue(resVal)}", textFormat, new RectangleF(70, counter * 20, 1000, 1000), brush, DrawTextOptions.None);

                if (resVal is SolidColorBrush brush1)
                {
                    using var brush2 = new SolidColorBrush(_renderTarget, brush1.Color);

                    _renderTarget.FillRectangle(new RectangleF(55, counter * 20 + 4, 12, 12), brush2);
                }

                counter++;
            }
        }

        _renderTarget.Transform = tmp;
    }
}
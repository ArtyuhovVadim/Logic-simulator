﻿using System;
using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.ExtensionMethods;
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

    public SceneRenderer(double pixelWidth, double pixelHeight, float dpi) => StartDirect3D(pixelWidth, pixelHeight, dpi);

    public bool IsRendering { get; set; } = true;

    public Matrix3x2 Transform => _renderTarget.Transform;

    public float Scale
    {
        get => Transform.M11;
        set => _renderTarget.Transform = _renderTarget.Transform with { M11 = value, M22 = value };
    }

    public Vector2 TranslationVector
    {
        get => new(_renderTarget.Transform.M31, _renderTarget.Transform.M32);
        set => _renderTarget.Transform = _renderTarget.Transform with { M31 = value.X, M32 = value.Y };
    }

    public void RelativeScale(Vector2 pos, float delta)
    {
        var p = pos.Transform(Transform);

        var newScaleCoefficient = 1 + delta / Scale;
        var newScale = (float)Math.Round(Scale * newScaleCoefficient, 2);

        if (newScale is < 0.5f or > 20f) return;

        TranslationVector += p * ((1 - newScaleCoefficient) * Scale);

        Scale = newScale;
    }

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

    public void Resize(double pixelWidth, double pixelHeight, float dpi)
    {
        CreateAndBindTargets(pixelWidth, pixelHeight, dpi);
        //TODO: Возможно стоит перенести вызов в другое место
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

        _objectRenderer = new ObjectRenderer(_renderTarget);
        _componentRenderer = new ComponentRenderer(_renderTarget);

        _imageSource.SetRenderTarget(_texture2D);

        _device.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height);
    }

    private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e) =>
        IsRendering = _imageSource.IsFrontBufferAvailable;
}
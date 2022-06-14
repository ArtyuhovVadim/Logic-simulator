using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace LogicSimulator.Scene;

public abstract class D2dControl : System.Windows.Controls.Image
{
    #region Private fields

    private SharpDX.Direct3D11.Device _device;
    private Texture2D _renderTarget;
    private Dx11ImageSource _imageSource;
    private RenderTarget _d2DRenderTarget;
    private SharpDX.Direct2D1.Factory _d2DFactory;

    private bool _isRendering;

    #endregion

    protected D2dControl()
    {
        Loaded += Window_Loaded;
        Unloaded += Window_Closing;

        Stretch = Stretch.Fill;
    }

    #region Private properties

    private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(this);

    #endregion

    public abstract void Render(RenderTarget target);

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        CreateAndBindTargets();
        base.OnRenderSizeChanged(sizeInfo);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (IsInDesignMode)
        {
            return;
        }

        StartD3D();
        StartRendering();
    }

    private void Window_Closing(object sender, RoutedEventArgs e)
    {
        if (IsInDesignMode)
        {
            return;
        }

        StopRendering();
        EndD3D();
    }

    private void OnRendering(object sender, EventArgs e)
    {
        if (!_isRendering)
        {
            return;
        }

        PrepareAndCallRender();
        _imageSource.InvalidateD3DImage();
    }

    private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (_imageSource.IsFrontBufferAvailable) StartRendering();
        else StopRendering();
    }

    private void StartD3D()
    {
        _device = new SharpDX.Direct3D11.Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);

        _imageSource = new Dx11ImageSource();
        _imageSource.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

        CreateAndBindTargets();

        Source = _imageSource;
    }

    private void EndD3D()
    {
        _imageSource.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;
        Source = null;

        Utilities.Dispose(ref _d2DRenderTarget);
        Utilities.Dispose(ref _d2DFactory);
        Utilities.Dispose(ref _imageSource);
        Utilities.Dispose(ref _renderTarget);
        Utilities.Dispose(ref _device);
    }

    private void CreateAndBindTargets()
    {
        if (_imageSource == null)
            return;

        _imageSource.SetRenderTarget(null);

        Utilities.Dispose(ref _d2DRenderTarget);
        Utilities.Dispose(ref _d2DFactory);
        Utilities.Dispose(ref _renderTarget);

        var width = Math.Max((int)ActualWidth, 100);
        var height = Math.Max((int)ActualHeight, 100);

        var renderDesc = new Texture2DDescription
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

        _renderTarget = new Texture2D(_device, renderDesc);

        var surface = _renderTarget.QueryInterface<Surface>();

        _d2DFactory = new SharpDX.Direct2D1.Factory();
        var rtp = new RenderTargetProperties(new PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied));
        _d2DRenderTarget = new RenderTarget(_d2DFactory, surface, rtp);

        _imageSource.SetRenderTarget(_renderTarget);

        _device.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height, 0.0f, 1.0f);
    }

    private void StartRendering()
    {
        if (_isRendering)
            return;

        CompositionTarget.Rendering += OnRendering;
        _isRendering = true;
    }

    private void StopRendering()
    {
        if (!_isRendering)
            return;

        CompositionTarget.Rendering -= OnRendering;
        _isRendering = false;
    }

    private void PrepareAndCallRender()
    {
        if (_device == null)
            return;

        _d2DRenderTarget.BeginDraw();

        Render(_d2DRenderTarget);

        _d2DRenderTarget.EndDraw();

        _device.ImmediateContext.Flush();
    }
}
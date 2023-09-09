using SharpDX;
using SharpDX.Direct2D1;
using DXGISurface = SharpDX.DXGI.Surface;
using DXGIResource = SharpDX.DXGI.Resource;
using D3D11Texture2D = SharpDX.Direct3D11.Texture2D;
using D2DDevice = SharpDX.Direct2D1.Device;
using D2DDeviceContext = SharpDX.Direct2D1.DeviceContext;
using D2DFactory = SharpDX.Direct2D1.Factory;
using D2DTextFactory = SharpDX.DirectWrite.Factory;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

namespace LogicSimulator.Scene.DirectX;

public class DirectXContext : DisposableObject
{
    private AntialiasMode _lastAntialiasMode = AntialiasMode.Aliased;
    private TextAntialiasMode _textAntialiasMode = TextAntialiasMode.Default;
    private Matrix3x2 _lastTransformMatrix = Matrix3x2.Identity;

    private D2DDevice? _d2DDevice;
    private D2DDeviceContext? _d2DDeviceContext;
    private D2DFactory? _d2DFactory;
    private D2DTextFactory? _d2DTextFactory;

    public bool IsInitialized { get; private set; }
    public D2DDeviceContext D2DDeviceContext => _d2DDeviceContext ?? throw new ApplicationException("DirectXContext is not initialized.");
    public D2DFactory D2DFactory => _d2DFactory ?? throw new ApplicationException("DirectXContext is not initialized.");
    public D2DTextFactory D2DTextFactory => _d2DTextFactory ?? throw new ApplicationException("DirectXContext is not initialized.");

    public bool Initialize(nint resourceHandle)
    {
        ThrowIfDisposed();

        if (resourceHandle == nint.Zero)
            return false;

        if (IsInitialized)
        {
            _lastAntialiasMode = _d2DDeviceContext!.AntialiasMode;
            _lastTransformMatrix = _d2DDeviceContext.Transform;
        }

        ReleaseResources();

        using var comObject = new ComObject(resourceHandle);
        using var resource = comObject.QueryInterface<DXGIResource>();
        using var texture = resource.QueryInterface<D3D11Texture2D>();
        using var surface = texture.QueryInterface<DXGISurface>();

        var creationProperties = new CreationProperties
        {
            Options = DeviceContextOptions.EnableMultithreadedOptimizations,
            ThreadingMode = ThreadingMode.MultiThreaded
        };

        _d2DDeviceContext = new D2DDeviceContext(surface, creationProperties)
        {
            AntialiasMode = _lastAntialiasMode,
            Transform = _lastTransformMatrix,
            TextAntialiasMode = _textAntialiasMode,
        };

        _d2DDevice = _d2DDeviceContext.Device;
        _d2DFactory = _d2DDeviceContext.Factory;
        _d2DTextFactory = new D2DTextFactory();

        IsInitialized = true;

        return true;
    }

    public bool TryCreateD2DContext(out D2DContext? context)
    {
        ThrowIfDisposed();

        context = null;

        if (!IsInitialized)
            return false;

        context = new D2DContext(this);

        return true;
    }

    public void ReleaseResources()
    {
        Utilities.Dispose(ref _d2DDeviceContext);
        Utilities.Dispose(ref _d2DDevice);
        Utilities.Dispose(ref _d2DFactory);
        Utilities.Dispose(ref _d2DTextFactory);

        IsInitialized = false;
    }

    protected override void Dispose(bool disposingManaged)
    {
        if (disposingManaged)
        {
            ReleaseResources();
        }

        base.Dispose(disposingManaged);
    }
}
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.DirectX;
using SharpDX;
using TqkLibrary.Wpf.Interop.DirectX;

namespace LogicSimulator.Scene;

public class SceneRenderer : DisposableObject
{
    private bool _renderAfterResize = true;

    private DirectXContext _context;
    private D2DContext? _d2dContext;
    private  D3D11Image _image;
    private readonly Action<DirectXContext> _onRender;

    public event Action<bool> IsFrontBufferAvailableChanged;

    public DirectXContext DirectXContext => _context;
    public D2DContext D2DContext => _d2dContext ?? throw new ApplicationException("D2DContext is not initialized.");

    public SceneRenderer(Action<DirectXContext> onRender, nint windowHandle)
    {
        _onRender = onRender;

        _image = new D3D11Image
        {
            WindowOwner = windowHandle,
            OnRender = InternalRender
        };

        _image.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

        _context = new DirectXContext();
    }

    public void WpfRender(DrawingContext drawingContext, Size renderSize)
    {
        if (_image is { IsFrontBufferAvailable: true })
        {
            drawingContext.DrawImage(_image, new Rect(renderSize));
        }
    }

    public void Resize(double width, double height, double dpi, bool renderAfterResize = true)
    {
        var pixelWidth = (int)Math.Max(Math.Round(width * dpi / 96f), 10);
        var pixelHeight = (int)Math.Max(Math.Round(height * dpi / 96f), 10);

        _renderAfterResize = renderAfterResize;

        _image.SetPixelSize(pixelWidth, pixelHeight);
    }

    public void Resize(Size newSize, double dpi, bool renderAfterResize = true) => Resize(newSize.Width, newSize.Height, dpi, renderAfterResize);

    public void RequestRender() => _image.RequestRender();

    private void InternalRender(nint resourceHandle, bool isNewSurface)
    {
        if (!_context.IsInitialized || isNewSurface)
        {
            Init(resourceHandle);
        }

        if (_renderAfterResize)
        {
            _onRender(_context);
        }

        _renderAfterResize = true;
    }

    private void Init(nint resourceHandle)
    {
        _d2dContext?.Dispose();

        if (!_context.Initialize(resourceHandle))
        {
            throw new ApplicationException("Can't create DirectX context");
        }

        if (!_context.TryCreateD2DContext(out _d2dContext))
        {
            throw new ApplicationException("Can't create Direct2D context");
        }
    }

    private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        try
        {
            IsFrontBufferAvailableChanged?.Invoke((bool)e.NewValue);
        }
        catch
        {
            Debug.Assert(false);
            throw;
        }
    }

    protected override void Dispose(bool disposingManaged)
    {
        if (disposingManaged)
        {
            _image.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;
            _image.OnRender = null;
            _image.WindowOwner = nint.Zero;

            Utilities.Dispose(ref _context);
            Utilities.Dispose(ref _d2dContext);
        }

        base.Dispose(disposingManaged);
    }
}
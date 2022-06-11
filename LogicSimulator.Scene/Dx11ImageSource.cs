using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using SharpDX;
using SharpDX.Direct3D9;

namespace LogicSimulator.Scene;

public class Dx11ImageSource : D3DImage, IDisposable
{
    private static int activeClients;
    private static Direct3DEx d3DContext;
    private static DeviceEx d3DDevice;

    #region Private fields

    private Texture _texture;

    #endregion

    public Dx11ImageSource()
    {
        StartD3D();
        activeClients++;
    }

    public void Dispose()
    {
        SetRenderTarget(null);

        Utilities.Dispose(ref _texture);

        activeClients--;
        EndD3D();
    }

    public void InvalidateD3DImage()
    {
        if (_texture == null) return;

        Lock();
        AddDirtyRect(new System.Windows.Int32Rect(0, 0, PixelWidth, PixelHeight));
        Unlock();
    }

    public void SetRenderTarget(SharpDX.Direct3D11.Texture2D target)
    {
        if (_texture != null)
        {
            _texture = null;

            Lock();
            SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
            Unlock();
        }

        if (target == null) return;

        var format = TranslateFormat(target);
        var handle = GetSharedHandle(target);

        if (!IsShareable(target)) throw new ArgumentException("Texture must be created with ResouceOptionFlags.Shared");

        if (format == Format.Unknown)
            throw new ArgumentException("Texture format is not compatible with OpenSharedResouce");

        if (handle == IntPtr.Zero) throw new ArgumentException("Invalid handle");

        _texture = new Texture(d3DDevice, target.Description.Width, target.Description.Height, 1,
            Usage.RenderTarget, format, Pool.Default, ref handle);

        using var surface = _texture.GetSurfaceLevel(0);
        Lock();
        SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
        Unlock();
    }

    private void StartD3D()
    {
        if (activeClients != 0) return;

        var presentParams = GetPresentParameters();
        var createFlags = CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve;

        d3DContext = new Direct3DEx();
        d3DDevice = new DeviceEx(d3DContext, 0, DeviceType.Hardware, IntPtr.Zero, createFlags, presentParams);
    }

    private void EndD3D()
    {
        if (activeClients != 0) return;

        Utilities.Dispose(ref _texture);
        Utilities.Dispose(ref d3DDevice);
        Utilities.Dispose(ref d3DContext);
    }

    private static void ResetD3D()
    {
        if (activeClients == 0) return;

        var presentParams = GetPresentParameters();
        d3DDevice.ResetEx(ref presentParams);
    }

    [DllImport("user32.dll", SetLastError = false)]
    private static extern IntPtr GetDesktopWindow();

    private static PresentParameters GetPresentParameters()
    {
        var presentParams = new PresentParameters
        {
            Windowed = true,
            SwapEffect = SwapEffect.Discard,
            DeviceWindowHandle = GetDesktopWindow(),
            PresentationInterval = PresentInterval.Default
        };

        return presentParams;
    }

    private IntPtr GetSharedHandle(SharpDX.Direct3D11.Texture2D texture)
    {
        using var resource = texture.QueryInterface<SharpDX.DXGI.Resource>();

        return resource.SharedHandle;
    }

    private static Format TranslateFormat(SharpDX.Direct3D11.Texture2D texture)
    {
        return texture.Description.Format switch
        {
            SharpDX.DXGI.Format.R10G10B10A2_UNorm => Format.A2B10G10R10,
            SharpDX.DXGI.Format.R16G16B16A16_Float => Format.A16B16G16R16F,
            SharpDX.DXGI.Format.B8G8R8A8_UNorm => Format.A8R8G8B8,
            _ => Format.Unknown
        };
    }

    private static bool IsShareable(SharpDX.Direct3D11.Texture2D texture)
    {
        return (texture.Description.OptionFlags & SharpDX.Direct3D11.ResourceOptionFlags.Shared) != 0;
    }
}
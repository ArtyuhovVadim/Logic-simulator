using System.Runtime.CompilerServices;

namespace LogicSimulator.Scene;

public abstract class DisposableObject : IDisposable
{
    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (IsDisposed)
            return;

        Dispose(true);
        GC.SuppressFinalize(this);

        IsDisposed = true;
    }

    protected virtual void Dispose(bool disposingManaged)
    {
        if (disposingManaged)
        {
            // освобождаем управляемые ресурсы
        }
        // освобождаем неуправляемые ресурсы
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ThrowIfDisposed()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(GetType().Name, $"{GetType().Name} object has been disposed.");
        }
    }
}
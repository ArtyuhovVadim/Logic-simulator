using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace LogicSimulator.Scene;

public abstract class DisposableFrameworkContentElement : FrameworkContentElement, IDisposable
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
            BindingOperations.ClearAllBindings(this);
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
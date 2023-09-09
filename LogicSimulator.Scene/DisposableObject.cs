using System.Runtime.CompilerServices;

namespace LogicSimulator.Scene;

/*
    Управляемые ресурсы - объекты реализующие IDisposable.
    Неуправляемые ресурсы - объекты о которых ничего не знает GC, например, выделенный участок памяти в куче при помощи Marshal.AllocHGlobal(...).

    1. Обратите внимание, что ресурсы должны освобождать только те классы, которым эти ресурсы принадлежат. 
    В частности, класс может иметь ссылку на общий ресурс — в этом случае вы не должны освобождать его,
    поскольку другие классы могут продолжать использовать этот ресурс.

    2. Для класса, владеющего управляемыми ресурсами, реализуйте IDisposable (но не финализатор)
    Размещение финализатора в классе, который владеет только управляемыми ресурсами, может приводить к ошибкам из-за вызова кода освобождения ресурсов из потока GC.
    Также финализатор вызывается не детерминировано, т.е. время и порядок вызова не могут быть предсказаны.

    3. Многократный вызов Dispose должен происходить без ошибок.

    4. Если реализован IDisposable, то в конце метода Dispose должен быть подавлен вызов финализатора. GC.SuppressFinalize(this).

    5. Для класса, владеющего неуправляемыми ресурсами, реализуйте IDisposable и финализатор.
*/

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
using System.Runtime.InteropServices;

namespace LogicSimulator.Utils;

public static class User32
{
    private const string DllName = "user32.dll";

    [DllImport(DllName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetCursorPos(int x, int y);
}
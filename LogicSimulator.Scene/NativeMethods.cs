using System;
using System.Runtime.InteropServices;

namespace LogicSimulator.Scene {
    public static class NativeMethods {
        [DllImport( "user32.dll", SetLastError = false )]
        public static extern IntPtr GetDesktopWindow();
    }
}

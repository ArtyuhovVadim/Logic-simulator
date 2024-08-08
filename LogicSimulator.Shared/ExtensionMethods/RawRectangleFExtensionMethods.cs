using SharpDX;
using SharpDX.Mathematics.Interop;

namespace LogicSimulator.Shared.ExtensionMethods;

public static class RawRectangleFExtensionMethods
{
    public static RectangleF ToRect(this RawRectangleF rect) => new() { Top = rect.Top, Bottom = rect.Bottom, Left = rect.Left, Right = rect.Right };
}
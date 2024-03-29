using SharpDX;

namespace LogicSimulator.Utils;

public static class RectangleFExtensionMethods
{
    public static RectangleF ScaleSize(this RectangleF rect, float scale) => rect with { Width = rect.Width * scale, Height = rect.Height * scale };
}
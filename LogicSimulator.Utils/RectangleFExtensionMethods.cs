using SharpDX;

namespace LogicSimulator.Utils;

public static class RectangleFExtensionMethods
{
    public static RectangleF ScaleSize(this RectangleF rect, float scale) => rect with { Width = rect.Width * scale, Height = rect.Height * scale };

    public static RectangleF ScaleAtCenter(this RectangleF rect, float scaleX, float scaleY)
    {
        var w = rect.Width * scaleX;
        var h = rect.Height * scaleY;
        var l = rect.Location - new Vector2((w - rect.Width) / 2, (h - rect.Height) / 2);
        return rect with { Location = l, Width = w, Height = h };
    }
}
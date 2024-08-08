using SharpDX;

namespace LogicSimulator.Shared.ExtensionMethods;

public static class RectangleFExtensionMethods
{
    public static bool IntersectsInclusive(this RectangleF a, RectangleF b) => a.Left <= b.Right && a.Right >= b.Left && a.Top <= b.Bottom && a.Bottom >= b.Top;

    public static bool Contains(this RectangleF a, RectangleF b) => a.X <= b.X && b.Right <= a.Right && a.Y <= b.Y && b.Bottom <= a.Bottom;

    public static bool IsNormal(this RectangleF rect) => !float.IsNaN(rect.Left) && !float.IsNaN(rect.Top) && !float.IsNaN(rect.Right) && !float.IsNaN(rect.Bottom) &&
                                                         !float.IsInfinity(rect.Left) && !float.IsInfinity(rect.Top) && !float.IsInfinity(rect.Right) && !float.IsInfinity(rect.Bottom);

    public static RectangleF ScaleSize(this RectangleF rect, float scale) => rect with { Width = rect.Width * scale, Height = rect.Height * scale };

    public static RectangleF ScaleAtCenter(this RectangleF rect, float scaleX, float scaleY)
    {
        var w = rect.Width * scaleX;
        var h = rect.Height * scaleY;
        var l = rect.Location - new Vector2((w - rect.Width) / 2, (h - rect.Height) / 2);
        return rect with { Location = l, Width = w, Height = h };
    }

    public static RectangleF Transform(this RectangleF rect, Matrix3x2 matrix)
    {
        var point0 = Matrix3x2.TransformPoint(matrix, rect.TopLeft);
        var point1 = Matrix3x2.TransformPoint(matrix, rect.TopRight);
        var point2 = Matrix3x2.TransformPoint(matrix, rect.BottomRight);
        var point3 = Matrix3x2.TransformPoint(matrix, rect.BottomLeft);

        var newRect = new RectangleF
        {
            X = Math.Min(Math.Min(point0.X, point1.X), Math.Min(point2.X, point3.X)),
            Y = Math.Min(Math.Min(point0.Y, point1.Y), Math.Min(point2.Y, point3.Y))
        };

        newRect.Width = Math.Max(Math.Max(point0.X, point1.X), Math.Max(point2.X, point3.X)) - newRect.X;
        newRect.Height = Math.Max(Math.Max(point0.Y, point1.Y), Math.Max(point2.Y, point3.Y)) - newRect.Y;

        return newRect;
    }

    public static RectangleF ToInflated(this RectangleF rect, Vector2 value)
    {
        rect.Inflate(value.X, value.Y);
        return rect;
    }

    public static RectangleF ToInflated(this RectangleF rect, float value)
    {
        rect.Inflate(value, value);
        return rect;
    }

    public static RectangleF Normalize(this RectangleF rect)
    {
        var x = rect.X;
        var y = rect.Y;
        var width = rect.Width;
        var height = rect.Height;

        if (width < 0)
        {
            width *= -1;
            x -= width;
        }

        if (height < 0)
        {
            height *= -1;
            y -= height;
        }

        return new RectangleF(x, y, width, height);
    }
}
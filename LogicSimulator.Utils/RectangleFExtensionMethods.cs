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
}
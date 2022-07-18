using SharpDX;
using SharpDX.Mathematics.Interop;

namespace LogicSimulator.Scene.ExtensionMethods;

public static class Vector2ExtensionMethods
{
    public static Vector2 Transform(this in Vector2 vector, Matrix3x2 matrix)
    {
        return new Vector2((vector.X - matrix.M31) / matrix.M11, (vector.Y - matrix.M32) / matrix.M11);
    }

    public static Vector2 ApplyGrid(this in Vector2 vector, float snap)
    {
        return new Vector2((int) (vector.X / snap + 0.5f) * snap, (int) (vector.Y / snap + 0.5f) * snap);
    }

    public static Vector2 DpiCorrect(this in Vector2 vector, float dpi)
    {
        return vector / (96f / dpi);
    }

    public static System.Windows.Point ToPoint(this in Vector2 vector)
    {
        return new System.Windows.Point(vector.X, vector.Y);
    }

    public static bool IsInRectangle(this Vector2 vector, RawRectangleF rect)
    {
        return (rect.Left >= vector.X && vector.X >= rect.Right || rect.Right >= vector.X && vector.X >= rect.Left) &&
               (rect.Top >= vector.Y && vector.Y >= rect.Bottom || rect.Bottom >= vector.Y && vector.Y >= rect.Top);
    }

    public static RawRectangleF RectangleRelativePointAsCenter(this Vector2 vector, float size) =>
        new(vector.X - size, vector.Y - size, vector.X + size, vector.Y + size);
}
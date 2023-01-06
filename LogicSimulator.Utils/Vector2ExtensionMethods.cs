using SharpDX;
using SharpDX.Mathematics.Interop;

namespace LogicSimulator.Utils;

public static class Vector2ExtensionMethods
{
    public static Vector2 RotateRelative(this in Vector2 vector, float angle, Vector2 offset)
    {
        var matrix = Matrix3x2.Transformation(1, 1, MathUtil.DegreesToRadians(angle), offset.X, offset.Y);
        return Matrix3x2.TransformPoint(matrix, vector - offset);
    }

    public static Vector2 Transform(this in Vector2 vector, Matrix3x2 matrix) =>
        Matrix3x2.TransformPoint(matrix, vector);

    public static Vector2 InvertAndTransform(this in Vector2 vector, Matrix3x2 matrix) =>
         Matrix3x2.TransformPoint(Matrix3x2.Invert(matrix), vector);

    public static Vector2 ApplyGrid(this in Vector2 vector, float snap) =>
        new((int)(vector.X / snap + 0.5f) * snap, (int)(vector.Y / snap + 0.5f) * snap);

    public static Vector2 DpiCorrect(this in Vector2 vector, float dpi) =>
        vector / (96f / dpi);

    public static System.Windows.Point ToPoint(this in Vector2 vector) =>
        new(vector.X, vector.Y);

    public static bool IsInRectangle(this in Vector2 vector, RawRectangleF rect) =>
        (rect.Left >= vector.X && vector.X >= rect.Right || rect.Right >= vector.X && vector.X >= rect.Left) &&
        (rect.Top >= vector.Y && vector.Y >= rect.Bottom || rect.Bottom >= vector.Y && vector.Y >= rect.Top);

    public static RawRectangleF RectangleRelativePointAsCenter(this in Vector2 vector, float size) =>
        new(vector.X - size, vector.Y - size, vector.X + size, vector.Y + size);
}
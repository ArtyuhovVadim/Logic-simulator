using SharpDX;

namespace LogicSimulator.Scene.ExtensionMethods;

public static class Vector2ExtensionMethods
{
    public static Vector2 Transform(this in Vector2 vector, Matrix3x2 matrix)
    {
        return new Vector2((vector.X - matrix.M31) / matrix.M11, (vector.Y - matrix.M32) / matrix.M11);
    }

    public static Vector2 DpiCorrect(this in Vector2 vector, float dpi)
    {
        return vector / (96f / dpi);
    }
}
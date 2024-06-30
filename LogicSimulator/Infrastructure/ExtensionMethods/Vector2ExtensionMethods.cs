using SharpDX;

namespace LogicSimulator.Infrastructure.ExtensionMethods;

public static class Vector2ExtensionMethods
{
    public static Vector2 Transform(this Vector2 vector, Rotation rotation) => rotation switch
    {
        Rotation.Degrees0 => vector,
        Rotation.Degrees90 => new Vector2(-vector.Y, vector.X),
        Rotation.Degrees180 => new Vector2(-vector.X, -vector.Y),
        Rotation.Degrees270 => new Vector2(vector.Y, -vector.X),
        _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
    };
}
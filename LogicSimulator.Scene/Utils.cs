using System;

namespace LogicSimulator.Scene;

public static class Utils
{
    public static int RotationToInt(Rotation rotation) => rotation switch
    {
        Rotation.Degrees0 => 0,
        Rotation.Degrees90 => 90,
        Rotation.Degrees180 => 180,
        Rotation.Degrees270 => 270,
        Rotation.Undefined => throw new ArgumentException("Undefined rotation!", nameof(rotation)),
        _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
    };

    public static int GetRotationDifference(Rotation rotationA, Rotation rotationB) => 
        Math.Abs(RotationToInt(rotationA) - RotationToInt(rotationB));

    public static Rotation GetNextRotation(Rotation rotation) => rotation switch
    {
        Rotation.Degrees0 => Rotation.Degrees90,
        Rotation.Degrees90 => Rotation.Degrees180,
        Rotation.Degrees180 => Rotation.Degrees270,
        Rotation.Degrees270 => Rotation.Degrees0,
        Rotation.Undefined => throw new ArgumentException("Undefined rotation!", nameof(rotation)),
        _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
    };
}
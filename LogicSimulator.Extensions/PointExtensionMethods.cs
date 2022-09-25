using SharpDX;

namespace LogicSimulator.Extensions;

public static class PointExtensionMethods
{
    public static Vector2 ToVector2(this System.Windows.Point point)
    {
        return new Vector2((float)point.X, (float)point.Y);
    }
}
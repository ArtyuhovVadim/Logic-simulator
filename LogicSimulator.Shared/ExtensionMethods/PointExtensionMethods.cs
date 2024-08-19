using SharpDX;
using Point = System.Windows.Point;

namespace LogicSimulator.Shared.ExtensionMethods;

public static class PointExtensionMethods
{
    public static Vector2 ToVector2(this Point point)
    {
        return new Vector2((float)point.X, (float)point.Y);
    }
}
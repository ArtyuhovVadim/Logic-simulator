using SharpDX;

namespace LogicSimulator.Scene;

public static class OriginPositionExtensionMethods
{
    public static Vector2 ToVector2(this OriginPosition origin, RectangleF bounds) => origin switch
    {
        OriginPosition.TopLeft => -bounds.TopLeft,
        OriginPosition.TopCenter => -new Vector2(bounds.Center.X, bounds.Top),
        OriginPosition.TopRight => -bounds.TopRight,
        OriginPosition.CenterLeft => -new Vector2(bounds.Left, bounds.Center.Y),
        OriginPosition.Center => -bounds.Center,
        OriginPosition.CenterRight => -new Vector2(bounds.Right, bounds.Center.Y),
        OriginPosition.BottomLeft => -bounds.BottomLeft,
        OriginPosition.BottomCenter => -new Vector2(bounds.Center.X, bounds.Bottom),
        OriginPosition.BottomRight => -bounds.BottomRight,
        _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null)
    };
}
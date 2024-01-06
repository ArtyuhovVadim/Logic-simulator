using SharpDX;

namespace LogicSimulator.Scene;

public static class MathHelper
{
    public static float NormalizeAngle(float angle)
    {
        float ang;
        for (ang = angle; ang > 360f; ang -= 360f) { }
        for (; ang < 0f; ang += 360f) { }
        return ang;
    }

    public static float Sqr(float x) => x * x;

    public static float Rads(float angle) => MathUtil.DegreesToRadians(angle);

    public static float Degrees(float angle) => MathUtil.RadiansToDegrees(angle);

    public static Vector2 GetPositionFromAngle(Vector2 pos, float width, float height, float angle)
    {
        if (width == 0f || height == 0f)
        {
            return pos;
        }
        var num = Rads(angle);
        var num2 = (float)Math.Cos(num);
        var num3 = (float)Math.Sin(num);
        var num4 = Math.Sqrt(1f / (Sqr(num2 / width) + Sqr(num3 / height)));
        return new Vector2((float)(pos.X + num4 * num2), (float)(pos.Y + num4 * num3));
    }

    public static float CosBetweenVectors(Vector2 v1, Vector2 v2)
    {
        return (v1.X * v2.X + v1.Y * v2.Y) / (v1.Length() * v2.Length());
    }

    public static float GetAngleForArc(Vector2 center, float radius, Vector2 mousePos)
    {
        var firstVector = new Vector2(center.X + radius, center.Y) - center;
        var secondVector = mousePos - center;
        var rawAngle = Degrees((float)Math.Acos(CosBetweenVectors(firstVector, secondVector)));
        var angle = mousePos.Y > center.Y ? 360 - rawAngle : rawAngle;

        return angle;
    }
}
using SharpDX;

namespace LogicSimulator.Utils;

public static class ColorHelper
{
    public static System.Windows.Media.Color ColorFromHsv(double h, double s, double v)
    {
        h = h.Clamp(0, 360);
        s = s.Clamp(0, 1);
        v = v.Clamp(0, 1);

        return System.Windows.Media.Color.FromRgb(F(5, h, s, v), F(3, h, s, v), F(1, h, s, v));
    }

    public static Color4 Color4FromHsv(double h, double s, double v)
    {
        return new Color4(F(5, h, s, v) / 255f, F(3, h, s, v) / 255f, F(1, h, s, v) / 255f, 1f);
    }

    private static byte F(double n, double h, double s, double v)
    {
        var k = (n + h / 60) % 6;
        var value = v - v * s * Math.Max(Math.Min(Math.Min(k, 4 - k), 1), 0);
        return (byte)Math.Round(value * 255);
    }
}
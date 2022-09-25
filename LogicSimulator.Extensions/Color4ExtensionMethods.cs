using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Extensions;

public static class Color4ExtensionMethods
{
    public static Color ToColor(this Color4 color)
    {
        var a = Convert.ToByte(color.Alpha * 255);
        var r = Convert.ToByte(color.Red * 255);
        var g = Convert.ToByte(color.Green * 255);
        var b = Convert.ToByte(color.Blue * 255);

        return Color.FromArgb(a, r, g, b);
    }

    //TODO: Оптимизировать
    public static (double h, double s, double v) AsHsv(this Color4 color)
    {
        var wpfColor = color.ToColor();
        var tmpColor = System.Drawing.Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);

        int max = Math.Max(tmpColor.R, Math.Max(tmpColor.G, tmpColor.B));
        int min = Math.Min(tmpColor.R, Math.Min(tmpColor.G, tmpColor.B));

        var hue = tmpColor.GetHue();
        var saturation = (max == 0) ? 0 : 1d - (1d * min / max);
        var value = max / 255d;

        return (hue, saturation, value);
    }
}
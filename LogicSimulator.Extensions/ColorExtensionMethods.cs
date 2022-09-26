using SharpDX;
using WpfColor = System.Windows.Media.Color;

namespace LogicSimulator.Extensions;

public static class ColorExtensionMethods
{
    public static Color4 ToColor4(this WpfColor color)
    {
        var a = (float)Convert.ToDouble(color.A / 255d);
        var r = (float)Convert.ToDouble(color.R / 255d);
        var g = (float)Convert.ToDouble(color.G / 255d);
        var b = (float)Convert.ToDouble(color.B / 255d);

        return new Color4(r, g, b, a);
    }
}
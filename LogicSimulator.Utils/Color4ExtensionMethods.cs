using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Utils;

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
}
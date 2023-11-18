using System.Globalization;
using LogicSimulator.Utils;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Infrastructure.Converters;

public class ColorToColor4Converter : Converter
{
    public override object Convert(object v, Type t, object p, CultureInfo c) => ((Color)v!).ToColor4();

    public override object ConvertBack(object v, Type t, object p, CultureInfo c) => ((Color4)v!).ToColor();
}
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using SharpDX;

namespace LogicSimulator.Infrastructure.Converters;

[ValueConversion(typeof(Vector2), typeof(Vector2))]
[MarkupExtensionReturnType(typeof(Vector2Converter))]
public class Vector2Converter : Converter
{
    public override object Convert(object v, Type t, object p, CultureInfo c)
    {
        var vec = (Vector2)v!;

        return $"X:{(int)vec.X} Y:{(int)vec.Y}";
    }
}
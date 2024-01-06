using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using SharpDX;
using WpfExtensions.Converters.Base;

namespace LogicSimulator.Infrastructure.Converters;

[ValueConversion(typeof(Vector2), typeof(Vector2))]
[MarkupExtensionReturnType(typeof(Vector2Converter))]
public class Vector2Converter : BaseConverter
{
    public string NumberSuffix { get; set; } = string.Empty;

    public double DisplayCoefficient { get; set; } = 1;

    public override object Convert(object? v, Type t, object? p, CultureInfo c)
    {
        var vec = (Vector2)v!;

        return string.Format(CultureInfo.InvariantCulture, "X:{0:0.#}{1} Y:{2:0.#}{3}", vec.X / DisplayCoefficient, NumberSuffix, vec.Y / DisplayCoefficient, NumberSuffix);
    }
}
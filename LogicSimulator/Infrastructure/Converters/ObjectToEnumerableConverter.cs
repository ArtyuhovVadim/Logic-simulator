using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using SharpDX;
using WpfExtensions.Converters.Base;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Infrastructure.Converters;

[ValueConversion(typeof(Color), typeof(Color4))]
[MarkupExtensionReturnType(typeof(ColorToColor4Converter))]
public class ObjectToEnumerableConverter : BaseConverter<object?, IEnumerable<object?>>
{
    public override IEnumerable<object?> Convert(object? value, object? parameter, CultureInfo culture) => [value];

    public override object? ConvertBack(IEnumerable<object?>? value, object? parameter, CultureInfo culture) => value?.First();
}

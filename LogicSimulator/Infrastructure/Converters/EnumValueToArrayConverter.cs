using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using WpfExtensions.Converters.Base;

namespace LogicSimulator.Infrastructure.Converters;

[ValueConversion(typeof(Enum), typeof(string[]))]
[MarkupExtensionReturnType(typeof(EnumValueToArrayConverter))]
public class EnumValueToArrayConverter : BaseConverter
{
    public override object Convert(object? v, Type t, object? p, CultureInfo c) => Enum.GetValues(v!.GetType());
}
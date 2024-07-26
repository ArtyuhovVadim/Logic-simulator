using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using WpfExtensions.Converters.Base;

namespace LogicSimulator.Infrastructure.Converters;

[ValueConversion(typeof(bool), typeof(bool))]
[MarkupExtensionReturnType(typeof(NotConverter))]
public class NotConverter : BaseConverter
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is bool b ? !b : Binding.DoNothing;

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value is bool b ? !b : Binding.DoNothing;
}
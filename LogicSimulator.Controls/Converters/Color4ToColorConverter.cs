using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using SharpDX;

namespace LogicSimulator.Controls.Converters;

public class Color4ToColorConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var color4 = (Color4)value!;

        var r = System.Convert.ToByte(255 * color4.Red);
        var g = System.Convert.ToByte(255 * color4.Green);
        var b = System.Convert.ToByte(255 * color4.Blue);
        var a = System.Convert.ToByte(255 * color4.Alpha);

        return System.Windows.Media.Color.FromArgb(a, r, g, b);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
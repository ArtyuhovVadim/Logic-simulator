using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using LogicSimulator.Extensions;

namespace LogicSimulator.Controls.Converters;

public class HueToColorConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double hue)
            throw new ArgumentException("Type not supported!");

        return ColorHelper.ColorFromHsv(hue, 1, 1);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
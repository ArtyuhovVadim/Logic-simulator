using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfExtensions.Converters.Base;

namespace LogicSimulator.Infrastructure.Converters;

[ValueConversion(typeof(Uri), typeof(ImageSource))]
[MarkupExtensionReturnType(typeof(UriToImageSourceConverter))]
public class UriToImageSourceConverter : BaseConverter
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Uri uri)
            return new BitmapImage(uri);

        return DependencyProperty.UnsetValue;
    }
}
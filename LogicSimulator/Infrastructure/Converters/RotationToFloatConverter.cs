using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using LogicSimulator.Models.Common;
using WpfExtensions.Converters.Base;

namespace LogicSimulator.Infrastructure.Converters;

[ValueConversion(typeof(Rotation), typeof(float))]
[MarkupExtensionReturnType(typeof(RotationToFloatConverter))]
public class RotationToFloatConverter : BaseConverter<Rotation, float>
{
    public override float Convert(Rotation value, object? parameter, CultureInfo culture) => value switch
    {
        Rotation.Degrees0 => 0,
        Rotation.Degrees90 => 90,
        Rotation.Degrees180 => 180,
        Rotation.Degrees270 => 270,
        _ => throw new ArgumentOutOfRangeException()
    };

    public override Rotation ConvertBack(float value, object? parameter, CultureInfo culture) => (int)Math.Abs(value) switch
    {
        0 => Rotation.Degrees0,
        90 => Rotation.Degrees90,
        180 => Rotation.Degrees180,
        270 => Rotation.Degrees270,
        _ => throw new ArgumentOutOfRangeException()
    };
}
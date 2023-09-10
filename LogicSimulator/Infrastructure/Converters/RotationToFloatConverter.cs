using System.Globalization;
using System.Windows;

namespace LogicSimulator.Infrastructure.Converters;

public class RotationToFloatConverter : Converter
{
    public override object Convert(object v, Type t, object p, CultureInfo c)
    {
        if (v is not Rotation rotation)
            return DependencyProperty.UnsetValue;

        return rotation switch
        {
            Rotation.Undefined => throw new NotSupportedException(),
            Rotation.Degrees0 => 0,
            Rotation.Degrees90 => 90,
            Rotation.Degrees180 => 180,
            Rotation.Degrees270 => 270,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override object ConvertBack(object v, Type t, object p, CultureInfo c)
    {
        if (v is not float rotation)
            return DependencyProperty.UnsetValue;

        return (int)Math.Abs(rotation) switch
        {
            0 => Rotation.Degrees0,
            90 => Rotation.Degrees90,
            180 => Rotation.Degrees180,
            270 => Rotation.Degrees270,
            _ => Rotation.Undefined
        };
    }
}
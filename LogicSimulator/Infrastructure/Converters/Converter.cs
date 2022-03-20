using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace LogicSimulator.Infrastructure.Converters;

public abstract class Converter : MarkupExtension, IValueConverter
{
    public abstract object Convert(object v, Type t, object p, CultureInfo c);

    public virtual object ConvertBack(object v, Type t, object p, CultureInfo c)
    {
        throw new NotSupportedException("Обратное преобразование не поддерживается");
    }

    public override object ProvideValue(IServiceProvider sp)
    {
        return this;
    }
}
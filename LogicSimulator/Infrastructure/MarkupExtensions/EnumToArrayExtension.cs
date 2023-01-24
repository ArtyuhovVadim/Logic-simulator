using System;
using System.Windows.Markup;

namespace LogicSimulator.Infrastructure.MarkupExtensions;

public class EnumToArrayExtension : MarkupExtension
{
    public Type EnumType { get; private set; }

    public EnumToArrayExtension(Type enumType) => EnumType = enumType;

    public override object ProvideValue(IServiceProvider serviceProvider) => Enum.GetValues(EnumType);
}
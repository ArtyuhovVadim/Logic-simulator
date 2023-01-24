using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace LogicSimulator.Infrastructure.MarkupExtensions;

public class SystemFontFamilies : MarkupExtension
{
    private static readonly IEnumerable<string> Fonts;

    static SystemFontFamilies()
    {
        Fonts = System.Windows.Media.Fonts.SystemFontFamilies.Select(x => x.Source);
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => Fonts;
}
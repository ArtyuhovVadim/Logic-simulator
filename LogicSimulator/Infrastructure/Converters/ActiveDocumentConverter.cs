using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using WpfExtensions.Converters.Base;

namespace LogicSimulator.Infrastructure.Converters;

[ValueConversion(typeof(DocumentViewModel), typeof(object))]
[MarkupExtensionReturnType(typeof(ActiveDocumentConverter))]
public class ActiveDocumentConverter : BaseConverter
{
    public override object Convert(object? v, Type t, object? p, CultureInfo c) =>
        v is DocumentViewModel ? v : Binding.DoNothing;

    public override object ConvertBack(object? v, Type t, object? p, CultureInfo c) =>
        v is DocumentViewModel ? v : Binding.DoNothing;
}
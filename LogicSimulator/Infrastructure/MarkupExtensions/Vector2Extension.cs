using System.Windows.Markup;
using SharpDX;

namespace LogicSimulator.Infrastructure.MarkupExtensions;

public class Vector2Extension : MarkupExtension
{
    public float X { get; set; }

    public float Y { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) => new Vector2(X, Y);
}
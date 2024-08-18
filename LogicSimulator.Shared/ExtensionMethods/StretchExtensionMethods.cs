using System.Windows.Media;
using SharpDX;

namespace LogicSimulator.Shared.ExtensionMethods;

public static class StretchExtensionMethods
{
    public static Vector2 ToScaleVector2(this Stretch stretch, float targetWidth, float targetHeight, RectangleF bounds) => stretch switch
    {
        Stretch.None => Vector2.One,
        Stretch.Fill => new Vector2(targetWidth / bounds.Width, targetHeight / bounds.Height),
        Stretch.Uniform => new Vector2(Math.Min(targetWidth / bounds.Width, targetHeight / bounds.Height)),
        Stretch.UniformToFill => new Vector2(Math.Max(targetWidth / bounds.Width, targetHeight / bounds.Height)),
        _ => throw new ArgumentOutOfRangeException(nameof(stretch), stretch, null)
    };
}
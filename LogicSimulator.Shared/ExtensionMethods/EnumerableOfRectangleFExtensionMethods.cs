using SharpDX;

namespace LogicSimulator.Shared.ExtensionMethods;

public static class EnumerableOfRectangleFExtensionMethods
{
    public static RectangleF GeometryUnion(this IEnumerable<RectangleF> rects)
    {
        var rectsArray = rects.ToArray();

        return rectsArray.Length switch
        {
            0 => RectangleF.Empty,
            1 => rectsArray[0],
            _ => rectsArray.Skip(1).Aggregate(rectsArray[0], RectangleF.Union)
        };
    }
}

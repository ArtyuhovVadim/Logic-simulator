using LogicSimulator.Scene;
using LogicSimulator.Scene.Models;

namespace LogicSimulator.Shared.ExtensionMethods;

public static class StrokedExtensionMethods
{
    public static float GetStrokeThickness(this IStroked obj) => obj.StrokeThicknessType switch
    {
        StrokeThicknessType.Smallest => 1f,
        StrokeThicknessType.Small => 1f,
        StrokeThicknessType.Medium => 3f,
        StrokeThicknessType.Large => 5f,
        StrokeThicknessType.Other => obj.StrokeThickness,
        _ => throw new InvalidOperationException("StrokeThicknessType out of range.")
    };

    public static float GetStrokeThickness(this IStroked obj, ITransformable transformable) => obj.StrokeThicknessType switch
    {
        StrokeThicknessType.Smallest => 1f / transformable.Scale,
        StrokeThicknessType.Small => Math.Max(1f, 1f / transformable.Scale),
        StrokeThicknessType.Medium => Math.Max(3f, 1f / transformable.Scale),
        StrokeThicknessType.Large => Math.Max(5f, 1f / transformable.Scale),
        StrokeThicknessType.Other => Math.Max(obj.StrokeThickness, 1f / transformable.Scale),
        _ => throw new InvalidOperationException("StrokeThicknessType out of range.")
    };
}
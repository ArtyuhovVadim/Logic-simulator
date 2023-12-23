using SharpDX;

namespace LogicSimulator.Scene;

public static class StrokedExtensionMethods
{
    public static float GetStrokeThickness(this IStroked obj) => obj.StrokeThicknessType switch
    {
        StrokeThicknessType.Smallest => 1f,
        StrokeThicknessType.Small => 2f,
        StrokeThicknessType.Medium => 3f,
        StrokeThicknessType.Large => 4f,
        StrokeThicknessType.Other => obj.StrokeThickness,
        _ => throw new InvalidOperationException("StrokeThicknessType out of range.")
    };

    public static float GetStrokeThickness(this IStroked obj, Scene2D scene) => obj.StrokeThicknessType switch
    {
        StrokeThicknessType.Smallest => 1f / scene.Scale,
        StrokeThicknessType.Small => Math.Max(2f, 1f / scene.Scale),
        StrokeThicknessType.Medium => Math.Max(3f, 1f / scene.Scale),
        StrokeThicknessType.Large => Math.Max(4f, 1f / scene.Scale),
        StrokeThicknessType.Other => Math.Max(obj.StrokeThickness, 1f / scene.Scale),
        _ => throw new InvalidOperationException("StrokeThicknessType out of range.")
    };
}
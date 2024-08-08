namespace LogicSimulator.Scene.Models;

public interface IStroked
{
    float StrokeThickness { get; }

    StrokeThicknessType StrokeThicknessType { get; }
}
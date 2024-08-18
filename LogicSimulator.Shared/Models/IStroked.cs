namespace LogicSimulator.Shared.Models;

public interface IStroked
{
    float StrokeThickness { get; }

    StrokeThicknessType StrokeThicknessType { get; }
}
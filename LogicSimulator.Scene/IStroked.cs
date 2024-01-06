namespace LogicSimulator.Scene;

public interface IStroked
{
    float StrokeThickness { get; }

    StrokeThicknessType StrokeThicknessType { get; }
}
using LogicSimulator.Scene;
using System.Windows.Media;
using LogicSimulator.Models.Base;

namespace LogicSimulator.Models;

public class RoundedRectangleModel : BaseObjectModel
{
    public float RadiusX { get; set; }

    public float RadiusY { get; set; }

    public float Width { get; set; }

    public float Height { get; set; }

    public Color FillColor { get; set; } = Colors.White;

    public Color StrokeColor { get; set; } = Colors.Black;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Smallest;

    public bool IsFilled { get; set; } = true;

    public override RoundedRectangleModel MakeClone() => (RoundedRectangleModel)MemberwiseClone();
}
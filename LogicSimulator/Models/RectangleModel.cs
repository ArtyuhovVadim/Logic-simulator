using System.Windows.Media;
using LogicSimulator.Models.Base;
using LogicSimulator.Scene;

namespace LogicSimulator.Models;

public class RectangleModel : BaseObjectModel
{
    public float Width { get; set; }

    public float Height { get; set; }

    public Color FillColor { get; set; } = Colors.White;

    public Color StrokeColor { get; set; } = Colors.Black;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Smallest;

    public bool IsFilled { get; set; } = true;

    public override RectangleModel MakeClone() => (RectangleModel)MemberwiseClone();
}
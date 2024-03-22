using System.Windows.Media;
using LogicSimulator.Models.Base;
using LogicSimulator.Scene;

namespace LogicSimulator.Models;

public class AndGateModel : BaseGateModel
{
    public Color FillColor { get; set; } = Colors.White;

    public Color StrokeColor { get; set; } = Colors.Black;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Smallest;

    public override AndGateModel MakeClone() => (AndGateModel)MemberwiseClone();
}
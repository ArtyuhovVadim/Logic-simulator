using System.Windows.Media;
using LogicSimulator.Models.Base;
using LogicSimulator.Scene;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Models;

public class WireModel : BaseObjectModel
{
    public List<Vector2> Vertexes { get; set; } = [];

    public Color StrokeColor { get; set; } = Colors.DarkBlue;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Small;

    public override WireModel MakeClone() => (WireModel)MemberwiseClone();
}
using System.Windows.Media;
using LogicSimulator.Models.Objects.Base;
using LogicSimulator.Scene.Models;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Models.Objects;

public class BezierCurveModel : BaseObjectModel
{
    public Vector2 Point1 { get; set; }

    public Vector2 Point2 { get; set; }

    public Vector2 Point3 { get; set; }

    public Color StrokeColor { get; set; } = Colors.Black;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Smallest;

    public override BezierCurveModel MakeClone() => (BezierCurveModel)MemberwiseClone();
}
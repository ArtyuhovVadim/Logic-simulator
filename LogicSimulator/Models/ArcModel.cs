using System.Windows.Media;
using LogicSimulator.Models.Base;
using LogicSimulator.Scene;

namespace LogicSimulator.Models;

public class ArcModel : BaseObjectModel
{
    public float RadiusX { get; set; }

    public float RadiusY { get; set; }

    public float StartAngle { get; set; }

    public float EndAngle { get; set; } = 180f;

    public Color StrokeColor { get; set; } = Colors.Black;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Smallest;

    public override ArcModel MakeClone() => (ArcModel)MemberwiseClone();
}
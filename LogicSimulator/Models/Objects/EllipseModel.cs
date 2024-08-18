using System.Windows.Media;
using LogicSimulator.Models.Objects.Base;
using LogicSimulator.Shared.Models;

namespace LogicSimulator.Models.Objects;

public class EllipseModel : BaseObjectModel
{
    public float RadiusX { get; set; }

    public float RadiusY { get; set; }

    public Color FillColor { get; set; } = Colors.White;

    public Color StrokeColor { get; set; } = Colors.Black;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Smallest;

    public bool IsFilled { get; set; } = true;

    public override EllipseModel MakeClone() => (EllipseModel)MemberwiseClone();
}
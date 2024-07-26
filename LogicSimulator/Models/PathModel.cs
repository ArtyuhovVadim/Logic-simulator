using System.Windows.Media;
using LogicSimulator.Models.Base;
using LogicSimulator.Scene;

namespace LogicSimulator.Models;

public class PathModel : BaseObjectModel
{
    public string Geometry { get; set; } = string.Empty;

    public float Width { get; set; } = float.NaN;

    public float Height { get; set; } = float.NaN;

    public float Scale { get; set; } = float.NaN;

    public OriginPosition OriginPosition { get; set; } = OriginPosition.Center;

    public Stretch Stretch { get; set; } = Stretch.None;

    public Color StrokeColor { get; set; } = Colors.Black;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Smallest;

    public Color FillColor { get; set; } = Colors.White;

    public bool IsAntiAliased { get; set; } = true;

    public bool IsStroked { get; set; } = true;

    public bool IsFilled { get; set; } = true;

    public override PathModel MakeClone() => (PathModel)MemberwiseClone();
}
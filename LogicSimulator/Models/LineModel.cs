using System.Windows.Media;
using LogicSimulator.Models.Base;
using LogicSimulator.Scene;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Models;

public class LineModel : BaseObjectModel
{
    public List<Vector2> Vertexes { get; set; } = [];

    public Color StrokeColor { get; set; } = Colors.Black;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Smallest;

    public override LineModel MakeClone()
    {
        var model = (LineModel)MemberwiseClone();
        model.Vertexes = [..Vertexes];
        return model;
    }
}
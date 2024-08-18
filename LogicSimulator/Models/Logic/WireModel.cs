using System.Windows.Media;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.Models.Common;
using LogicSimulator.Models.Objects.Base;
using LogicSimulator.Scene.Models;
using SharpDX;
using YamlDotNet.Serialization;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Models.Logic;

public class WireModel : BaseObjectModel
{
    public List<Vector2> Vertexes { get; set; } = [];

    [YamlIgnore]
    public IReadOnlyList<Vector2> AbsoluteVertexes => [Location, .. Vertexes.Select(x => Location + x.Transform(Rotation))];

    [YamlIgnore]
    public IReadOnlyList<Segment> Segments
    {
        get
        {
            var absoluteVertexes = AbsoluteVertexes;
            var segments = new List<Segment>();
            for (var i = 0; i < absoluteVertexes.Count - 1; i++)
                segments.Add(new Segment(absoluteVertexes[i], absoluteVertexes[i + 1]));
            return segments;
        }
    }

    public Color StrokeColor { get; set; } = Colors.DarkBlue;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Small;

    public static bool IsWiresConnected(WireModel wireA, WireModel wireB)
    {
        var wireAAbsoluteVertexes = wireA.AbsoluteVertexes.ToArray();
        var wireBAbsoluteVertexes = wireB.AbsoluteVertexes.ToArray();

        if (wireAAbsoluteVertexes.Any(x => wireBAbsoluteVertexes.Contains(x)))
            return true;

        if (wireAAbsoluteVertexes.Any(p => wireB.Segments.Any(seg => seg.ContainsPoint(p))))
            return true;

        if (wireBAbsoluteVertexes.Any(p => wireA.Segments.Any(seg => seg.ContainsPoint(p))))
            return true;

        return false;
    }

    public bool IsWireConnectedWith(WireModel other) => IsWiresConnected(this, other);

    public override WireModel MakeClone() => (WireModel)MemberwiseClone();
}
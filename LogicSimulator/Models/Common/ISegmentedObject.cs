using SharpDX;

namespace LogicSimulator.Models.Common;

public interface ISegmentedObject
{
    ObservableCollection<Vector2> Vertexes { get; }
}
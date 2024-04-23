using SharpDX;

namespace LogicSimulator.Models;

public interface ISegmentedObject
{
    ObservableCollection<Vector2> Vertexes { get; }
}
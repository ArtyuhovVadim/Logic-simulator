using System.Collections;
using SharpDX.Direct2D1;

namespace LogicSimulator.Shared;

public record RectHitTestResult<T>(IList<(GeometryRelation GeometryRelation, T Obj)> Objects) : IEnumerable<(GeometryRelation GeometryRelation, T Obj)> where T : IHitTestable
{
    public bool IsEmpty => Objects.Count == 0;

    public IEnumerator<(GeometryRelation GeometryRelation, T Obj)> GetEnumerator() => Objects.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
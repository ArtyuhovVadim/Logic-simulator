using System.Collections;

namespace LogicSimulator.Shared.Models.HitTest;

public record HitTestResult<T>(IList<T> Objects) : IEnumerable<T> where T : IHitTestable
{
    public bool IsEmpty => Objects.Count == 0;

    public IEnumerator<T> GetEnumerator() => Objects.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
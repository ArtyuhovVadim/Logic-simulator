using SharpDX;

namespace LogicSimulator.Shared.Models.HitTest;

public interface IHitTester
{
    IEnumerable<IHitTestable> Objects { get; }

    HitTestResult<T> HitTest<T>(Vector2 pos, float tolerance) where T : IHitTestable;

    RectHitTestResult<T> HitTest<T>(RectangleF rect) where T : IHitTestable;

    HitTestResult<T> HitTest<T>(Vector2 pos) where T : IHitTestable;

    HitTestResult<T> HitTestByBounds<T>(Vector2 pos) where T : IHitTestable;
}
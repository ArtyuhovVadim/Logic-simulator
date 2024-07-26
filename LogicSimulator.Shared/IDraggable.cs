using SharpDX;

namespace LogicSimulator.Shared;

public interface IDraggable : IHitTestable
{
    bool IsSelected { get; }

    bool IsDragging { get; }

    void StartDrag(Vector2 pos);

    void Drag(Vector2 pos);

    void EndDrag();
}
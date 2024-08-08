using LogicSimulator.Shared.Models.HitTest;
using SharpDX;

namespace LogicSimulator.Shared.Models;

public interface IDraggable : IHitTestable
{
    bool IsSelected { get; }

    bool IsDragging { get; }

    void StartDrag(Vector2 pos);

    void Drag(Vector2 pos);

    void EndDrag();
}
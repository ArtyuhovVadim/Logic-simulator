using SharpDX;

namespace LogicSimulator.Shared.Models.HitTest;

public interface IHitTestable
{
    bool IsMeasured { get; }

    Vector2 Location { get; set; }

    float Rotation { get; set; }

    RectangleF WorldBounds { get; }
}
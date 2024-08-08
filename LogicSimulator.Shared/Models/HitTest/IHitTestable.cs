using SharpDX;

namespace LogicSimulator.Shared.Models.HitTest;

public interface IHitTestable
{
    Vector2 Location { get; set; }

    float Rotation { get; set; }

    RectangleF WorldBounds { get; }
}
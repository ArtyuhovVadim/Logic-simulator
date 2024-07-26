using SharpDX;

namespace LogicSimulator.Shared;

public interface IHitTestable
{
    Vector2 Location { get; set; }

    float Rotation { get; set; }

    RectangleF WorldBounds { get; }
}
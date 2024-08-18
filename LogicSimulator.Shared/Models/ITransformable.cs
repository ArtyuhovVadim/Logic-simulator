using SharpDX;

namespace LogicSimulator.Shared.Models;

public interface ITransformable
{
    Matrix3x2 Transform { get; }

    float Scale { get; set; }

    Vector2 Translation { get; set; }

    float Rotation { get; set; }
}
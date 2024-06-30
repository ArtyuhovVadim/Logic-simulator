using LogicSimulator.Infrastructure;
using SharpDX;

namespace LogicSimulator.Models.Base;

public abstract class BaseObjectModel : ICloneable<BaseObjectModel>
{
    public Vector2 Location { get; set; } = Vector2.Zero;

    public Rotation Rotation { get; set; } = Rotation.Degrees0;

    public abstract BaseObjectModel MakeClone();
}
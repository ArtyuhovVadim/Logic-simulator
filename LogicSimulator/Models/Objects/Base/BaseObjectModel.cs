using LogicSimulator.Models.Common;
using LogicSimulator.Shared.Models;
using SharpDX;

namespace LogicSimulator.Models.Objects.Base;

public abstract class BaseObjectModel : ICloneable<BaseObjectModel>
{
    public Vector2 Location { get; set; } = Vector2.Zero;

    public Rotation Rotation { get; set; } = Rotation.Degrees0;

    public abstract BaseObjectModel MakeClone();
}
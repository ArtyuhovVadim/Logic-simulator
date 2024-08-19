using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Models.Objects.Base;
using SharpDX;
using YamlDotNet.Serialization;

namespace LogicSimulator.Models.Logic;

public class PortModel : BaseObjectModel
{
    public PortModel() { }

    public PortModel(BaseGateModel parent) => Parent = parent;

    public string Name { get; set; } = string.Empty;

    public float Length { get; set; } = 20f;

    [YamlIgnore]
    public Vector2 AbsoluteLocation => Parent.Location + (Location + new Vector2(Length, 0).Transform(Rotation)).Transform(Parent.Rotation);

    [YamlIgnore]
    public BaseGateModel Parent { get; set; } = null!;

    public PortModel MakeClone(BaseGateModel parent)
    {
        var clone = MakeClone();
        clone.Parent = parent;
        return clone;
    }

    public override PortModel MakeClone()
    {
        var port = (PortModel)MemberwiseClone();
        port.Parent = null!;
        return port;
    }
}
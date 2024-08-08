using LogicSimulator.Models.Objects.Base;

namespace LogicSimulator.Models.Logic;

public class PortModel : BaseObjectModel
{
    public string Name { get; set; } = string.Empty;

    public float Length { get; set; } = 20f;

    public override PortModel MakeClone() => (PortModel)MemberwiseClone();
}
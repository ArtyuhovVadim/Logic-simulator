using YamlDotNet.Serialization;

namespace LogicSimulator.Models.Logic.Gates.Base;

public abstract class SimpleGateModel : BaseGateModel
{
    protected SimpleGateModel()
    {
        OutputPort = new PortModel(this) { Name = "OUT0" };
        InputPortsList = [new PortModel(this) { Name = "IN0" }, new PortModel(this) { Name = "IN1" }];
    }

    public PortModel OutputPort { get; set; }

    [YamlMember(Alias = "InputPorts")]
    public List<PortModel> InputPortsList { get; set; }

    public override IEnumerable<PortModel> InputPorts => InputPortsList;

    public override IEnumerable<PortModel> OutputPorts => [OutputPort];

    public float InputPortsSpacing { get; set; } = 20f;
}
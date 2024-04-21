using YamlDotNet.Serialization;

namespace LogicSimulator.Models.Base;

public abstract class SimpleGateModel : BaseGateModel
{
    public List<PortModel> InputPorts { get; set; } = [new PortModel(), new PortModel()];

    [YamlIgnore]
    public int InputPortsCount => InputPorts.Count;

    public float InputPortsSpacing { get; set; } = 20f;
}
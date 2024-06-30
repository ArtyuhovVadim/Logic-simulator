namespace LogicSimulator.Models.Base;

public abstract class SimpleGateModel : BaseGateModel
{
    public PortModel OutputPort { get; set; } = new();

    public List<PortModel> InputPorts { get; set; } = [new PortModel(), new PortModel()];

    public float InputPortsSpacing { get; set; } = 20f;
}
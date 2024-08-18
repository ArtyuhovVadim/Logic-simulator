using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Logic.Gates.Base;

namespace LogicSimulator.Models.Logic.Gates;

public class OutputGateModel : BaseGateModel
{
    public OutputGateModel()
    {
        InputPort = new PortModel(this) { Name = "IN0" };
    }

    public PortModel InputPort { get; private set; }

    public override IEnumerable<PortModel> InputPorts => [InputPort];

    public override void Accept(IGateModelVisitor visitor) => visitor.Visit(this);

    public override OutputGateModel MakeClone()
    {
        var model = (OutputGateModel)MemberwiseClone();
        model.InputPort = InputPort.MakeClone();
        return model;
    }
}
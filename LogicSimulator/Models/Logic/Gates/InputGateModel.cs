using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Logic.Gates.Base;

namespace LogicSimulator.Models.Logic.Gates;

public class InputGateModel : BaseGateModel
{
    public InputGateModel()
    {
        OutputPort = new PortModel(this) { Name = "OUT0" };
    }

    public PortModel OutputPort { get; private set; }

    public override IEnumerable<PortModel> OutputPorts => [OutputPort];

    public override void Accept(IGateModelVisitor visitor) => visitor.Visit(this);

    public override InputGateModel MakeClone()
    {
        var model = (InputGateModel)MemberwiseClone();
        model.OutputPort = OutputPort.MakeClone(model);
        return model;
    }
}
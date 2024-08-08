using LogicSimulator.Models.Logic.Gates.Base;

namespace LogicSimulator.Models.Logic.Gates;

public class InputGateModel : BaseGateModel
{
    public PortModel OutputPort { get; private set; } = new();

    public override InputGateModel MakeClone()
    {
        var model = (InputGateModel)MemberwiseClone();
        model.OutputPort = OutputPort.MakeClone();
        return model;
    }
}
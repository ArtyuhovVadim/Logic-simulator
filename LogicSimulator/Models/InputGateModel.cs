using LogicSimulator.Models.Base;

namespace LogicSimulator.Models;

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
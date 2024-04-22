using LogicSimulator.Models.Base;

namespace LogicSimulator.Models;

public class OutputGateModel : BaseGateModel
{
    public PortModel InputPort { get; private set; } = new();

    public override OutputGateModel MakeClone()
    {
        var model = (OutputGateModel)MemberwiseClone();
        model.InputPort = InputPort.MakeClone();
        return model;
    }
}
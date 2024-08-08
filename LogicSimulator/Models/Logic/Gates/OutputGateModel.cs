using LogicSimulator.Models.Logic.Gates.Base;

namespace LogicSimulator.Models.Logic.Gates;

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
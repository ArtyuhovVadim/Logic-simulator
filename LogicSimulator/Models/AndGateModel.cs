using LogicSimulator.Models.Base;

namespace LogicSimulator.Models;

public class AndGateModel : SimpleGateModel
{
    public override AndGateModel MakeClone()
    {
        var model = (AndGateModel)MemberwiseClone();
        model.OutputPort = OutputPort.MakeClone();
        return model;
    }
}
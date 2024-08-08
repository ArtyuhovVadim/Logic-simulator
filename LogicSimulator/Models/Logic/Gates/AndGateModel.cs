using LogicSimulator.Models.Logic.Gates.Base;

namespace LogicSimulator.Models.Logic.Gates;

public class AndGateModel : SimpleGateModel
{
    public override AndGateModel MakeClone()
    {
        var model = (AndGateModel)MemberwiseClone();
        model.OutputPort = OutputPort.MakeClone();
        return model;
    }
}
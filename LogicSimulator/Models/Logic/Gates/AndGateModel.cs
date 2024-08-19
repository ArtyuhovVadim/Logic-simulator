using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Logic.Gates.Base;

namespace LogicSimulator.Models.Logic.Gates;

public class AndGateModel : SimpleGateModel
{
    public override void Accept(IGateModelVisitor visitor) => visitor.Visit(this);

    public override AndGateModel MakeClone()
    {
        var model = (AndGateModel)MemberwiseClone();
        model.InputPortsList = InputPortsList.Select(x => x.MakeClone(model)).ToList();
        model.OutputPort = OutputPort.MakeClone(model);
        return model;
    }
}
using LogicSimulator.Core.Gates;
using LogicSimulator.Models.Base;
using YamlDotNet.Serialization;

namespace LogicSimulator.Models;

public class AndGateModel : SimpleGateModel
{
    public AndGateModel() : base(new AndGate()) { }

    [YamlIgnore]
    public override AndGate LogicModel => (AndGate)base.LogicModel;

    public override AndGateModel MakeClone() => throw new NotImplementedException();
}
using LogicSimulator.Core.Gates.Base;
using LogicSimulator.Models.Logic.Gates.Base;

namespace LogicSimulator.Models.SchemeGraph.Base;

public interface ISchemeGraphGateNode<out TLogicModel, out TGateModel> : ISchemeGraphGateNode
    where TLogicModel : BaseGate
    where TGateModel : BaseGateModel
{
    new TGateModel GateModel { get; }

    new TLogicModel LogicModel { get; }
}
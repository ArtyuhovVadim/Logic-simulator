using LogicSimulator.Core.Gates.Base;
using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Models.SchemeGraph.Base;

namespace LogicSimulator.Models.SchemeGraph;

public class SchemeGraphGateNode<TLogicModel, TGateModel> : SchemeGraphGateNode, ISchemeGraphGateNode<TLogicModel, TGateModel>
    where TLogicModel : BaseGate
    where TGateModel : BaseGateModel
{
    public SchemeGraphGateNode(ISchemeGraphGateNode node) : this((TGateModel)node.GateModel, (TLogicModel)node.LogicModel) { }

    public SchemeGraphGateNode(TGateModel model, TLogicModel logicModel) : base(model)
    {
        GateModel = model;
        LogicModel = logicModel;
    }

    public new TGateModel GateModel { get; }

    public new TLogicModel LogicModel { get; }
}
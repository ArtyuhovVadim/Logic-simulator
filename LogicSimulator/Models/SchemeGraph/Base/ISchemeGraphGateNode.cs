using LogicSimulator.Core.Gates.Base;
using LogicSimulator.Models.Logic.Gates.Base;

namespace LogicSimulator.Models.SchemeGraph.Base;

public interface ISchemeGraphGateNode
{
    BaseGateModel GateModel { get; }

    BaseGate LogicModel { get; }

    IEnumerable<ISchemeGraphPortNode> InputPortNodes { get; }

    IEnumerable<ISchemeGraphPortNode> OutputPortNodes { get; }

    IEnumerable<ISchemeGraphPortNode> PortNodes { get; }
}
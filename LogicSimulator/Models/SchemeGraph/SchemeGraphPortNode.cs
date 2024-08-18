using System.Diagnostics;
using LogicSimulator.Core;
using LogicSimulator.Models.Common;
using LogicSimulator.Models.Logic;
using LogicSimulator.Models.SchemeGraph.Base;

namespace LogicSimulator.Models.SchemeGraph;

[DebuggerDisplay("\"{Parent.GateModel.Name,nq}/{PortModel.Name,nq}\"")]
public class SchemeGraphPortNode : ISchemeGraphPortNode
{
    public SchemeGraphPortNode(SchemeGraphGateNode parent, PortModel portModel)
    {
        Parent = parent;
        PortModel = portModel;
    }

    public PortType Type
    {
        get
        {
            if (Parent.InputPortNodes.Contains(this))
                return PortType.Input;

            if (Parent.OutputPortNodes.Contains(this))
                return PortType.Output;

            throw new InvalidOperationException("Unknown port type.");
        }
    }

    public HashSet<SchemeGraphEdge> Connections { get; } = [];

    public SchemeGraphGateNode Parent { get; }

    public PortModel PortModel { get; }

    public BasePort LogicModel { get; set; } = null!;

    IEnumerable<ISchemeGraphEdge> ISchemeGraphPortNode.Connections => Connections;

    ISchemeGraphGateNode ISchemeGraphPortNode.Parent => Parent;
}
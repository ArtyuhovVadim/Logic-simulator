using LogicSimulator.Core.Gates;
using LogicSimulator.Models.Logic;
using LogicSimulator.Models.Logic.Gates;
using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Models.Objects.Base;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Common;
using LogicSimulator.Models.SchemeGraph;

namespace LogicSimulator.Infrastructure.Services;

public class SchemePreprocessorService : ISchemePreprocessorService
{
    public IPreprocessedLogicScheme Process(IReadOnlyList<BaseObjectModel> objects)
    {
        var gates = objects.OfType<BaseGateModel>().ToList();
        var wires = objects.OfType<WireModel>().ToList();

        var wiresGroups = GroupWires(wires);
        var result = CreateSchemeGraph(gates, wiresGroups);
        CreateAndSetLogicModelsInNodes(result.Graph);

        return new PreprocessedLogicScheme(gates, result.Graph, result.InvalidConnection, result.Edges, wiresGroups);
    }

    private List<WiresGroup> GroupWires(List<WireModel> wires)
    {
        var graph = wires.ToDictionary(x => x, _ => new HashSet<WireModel>());

        foreach (var (wire, others) in graph)
        {
            foreach (var other in wires.Where(other => wire != other && wire.IsWireConnectedWith(other)))
            {
                others.Add(other);
            }
        }

        var visited = new HashSet<WireModel>();
        var groups = new List<WiresGroup>();

        foreach (var wire in wires)
        {
            if (visited.Contains(wire)) continue;

            var group = new WiresGroup();
            var stack = new Stack<WireModel>();
            stack.Push(wire);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (visited.Add(current))
                {
                    group.Wires.Add(current);

                    foreach (var neighbor in graph[current])
                    {
                        if (!visited.Contains(neighbor))
                        {
                            stack.Push(neighbor);
                        }
                    }
                }
            }

            groups.Add(group);
        }

        return groups;
    }

    private SchemeGraphGenerationResult CreateSchemeGraph(List<BaseGateModel> gates, List<WiresGroup> wireGroups)
    {
        var graph = gates.ToDictionary(x => new SchemeGraphGateNode(x), _ => new HashSet<SchemeGraphGateNode>());
        var edges = new List<SchemeGraphEdge>();
        var invalidConnections = new List<InvalidConnection>();

        foreach (var wiresGroup in wireGroups)
        {
            var connectedPorts = GetConnectedPorts(wiresGroup, graph.Keys);

            var outputPorts = connectedPorts.Where(x => x.Type == PortType.Output).ToArray();
            var inputPorts = connectedPorts.Where(x => x.Type == PortType.Input).ToArray();

            if (outputPorts.Length == 0 || inputPorts.Length == 0)
            {
                invalidConnections.Add(new InvalidConnection([.. inputPorts], [.. outputPorts], wiresGroup));
                continue;
            }

            foreach (var outputPort in outputPorts)
            {
                foreach (var inputPort in inputPorts)
                {
                    var edge = new SchemeGraphEdge(outputPort, inputPort, wiresGroup);
                    outputPort.Connections.Add(edge);
                    inputPort.Connections.Add(edge);
                    graph[outputPort.Parent].Add(inputPort.Parent);
                    graph[inputPort.Parent].Add(outputPort.Parent);
                    edges.Add(edge);
                }
            }
        }

        return new SchemeGraphGenerationResult(graph, edges, invalidConnections);
    }

    private void CreateAndSetLogicModelsInNodes(Dictionary<SchemeGraphGateNode, HashSet<SchemeGraphGateNode>> graph)
    {
        var factory = new GateLogicModelsFactory();

        foreach (var node in graph.Keys)
        {
            factory.CreateAndSetLogicModelInNode(node);
        }
    }

    private List<SchemeGraphPortNode> GetConnectedPorts(WiresGroup group, IEnumerable<SchemeGraphGateNode> gates)
    {
        var ports = new List<SchemeGraphPortNode>();

        foreach (var gate in gates)
        {
            ports.AddRange(gate.PortNodes.Where(port => group.ContainsPoint(port.PortModel.AbsoluteLocation)));
        }

        return ports;
    }

    private record SchemeGraphGenerationResult(
        Dictionary<SchemeGraphGateNode, HashSet<SchemeGraphGateNode>> Graph,
        List<SchemeGraphEdge> Edges,
        List<InvalidConnection> InvalidConnection);

    private class GateLogicModelsFactory : IGateModelVisitor
    {
        private SchemeGraphGateNode _currentGateNode = null!;

        public void CreateAndSetLogicModelInNode(SchemeGraphGateNode gateNode)
        {
            _currentGateNode = gateNode;
            gateNode.GateModel.Accept(this);
            _currentGateNode = null!;
        }

        public void Visit(InputGateModel gate)
        {
            var logicModel = new InputGate { Delay = gate.Delay };
            _currentGateNode.LogicModel = logicModel;
            _currentGateNode.OutputPortNodes.First().LogicModel = logicModel.Output;
        }

        public void Visit(OutputGateModel gate)
        {
            var logicModel = new OutputGate();
            _currentGateNode.LogicModel = logicModel;
            _currentGateNode.InputPortNodes.First().LogicModel = logicModel.Input;
        }

        public void Visit(AndGateModel gate)
        {
            var logicModel = new AndGate { Delay = gate.Delay, InputPortsCount = gate.InputPortsList.Count };
            _currentGateNode.LogicModel = logicModel;

            foreach (var (inputPortNode, logicPortModel) in _currentGateNode.InputPortNodes.Zip(logicModel.Inputs))
                inputPortNode.LogicModel = logicPortModel;

            _currentGateNode.OutputPortNodes.First().LogicModel = logicModel.Output;
        }
    }
}
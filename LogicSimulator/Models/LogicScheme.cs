using LogicSimulator.Core;
using LogicSimulator.Core.Gates;
using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Models;

public class LogicScheme
{
    public LogicScheme(List<BaseGate> gates, List<InputGate> inputGates, List<OutputGate> outputGates, List<Connection> connections, Dictionary<BaseGate, GateLogicModelToViewModelLink> gatesMap)
    {
        Gates = gates;
        InputGates = inputGates;
        OutputGates = outputGates;
        Connections = connections;
        GatesMap = gatesMap;
    }

    public List<BaseGate> Gates { get; set; }

    public List<InputGate> InputGates { get; set; }

    public List<OutputGate> OutputGates { get; set; }

    public List<Connection> Connections { get; set; }

    public Dictionary<BaseGate, GateLogicModelToViewModelLink> GatesMap { get; set; }
}
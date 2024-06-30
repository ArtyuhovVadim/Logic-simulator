using LogicSimulator.Core;
using LogicSimulator.Core.Gates;
using LogicSimulator.Core.Gates.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Gates;
using Microsoft.Extensions.Logging;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;

namespace LogicSimulator.Infrastructure.Services;

public class SchemeBuilderService : ISchemeBuilderService
{
    private readonly ILogger<SchemeBuilderService> _logger;
    private readonly List<Connection> _connections = [];
    private readonly Dictionary<BaseGate, GateLogicModelToViewModelLink> _logicModelsMap = [];

    public SchemeBuilderService(ILogger<SchemeBuilderService> logger) => _logger = logger;

    public LogicScheme BuildFromViewModels(IEnumerable<BaseObjectViewModel> objects)
    {
        try
        {
            _connections.Clear();
            _logicModelsMap.Clear();

            var objectsList = objects.ToList();
            var gates = objectsList.OfType<BaseGateViewModel>().ToList();
            var wires = objectsList.OfType<WireViewModel>().ToList();

            foreach (var gate in gates)
                gate.AcceptSchemeBuilder(this);

            foreach (var port in gates.SelectMany(x => x.Ports))
                port.State = SignalType.Undefined;

            var connectionPoints = wires.Select(x => (First: x.AbsoluteVertexes.First(), Last: x.AbsoluteVertexes.Last())).ToList();

            foreach (var connectionPoint in connectionPoints)
            {
                List<BasePort> firstPorts = [];
                List<BasePort> lastPorts = [];

                foreach (var (gateModel, gateLink) in _logicModelsMap)
                {
                    foreach (var (portModel, portLink) in gateLink.PortsMap)
                    {
                        if (connectionPoint.First == portLink.ViewModel.AbsoluteLocation)
                        {
                            firstPorts.Add(portModel);
                        }
                        else if (connectionPoint.Last == portLink.ViewModel.AbsoluteLocation)
                        {
                            lastPorts.Add(portModel);
                        }
                    }
                }

                if (firstPorts.Count != 1)
                    throw new InvalidOperationException();

                if (lastPorts.Count != 1)
                    throw new InvalidOperationException();

                InputPort inputPort;
                OutputPort outputPort;

                if (firstPorts[0] is InputPort inputPort0 && lastPorts[0] is OutputPort outputPort0)
                {
                    inputPort = inputPort0;
                    outputPort = outputPort0;
                }
                else if (firstPorts[0] is OutputPort outputPort1 && lastPorts[0] is InputPort inputPort1)
                {
                    inputPort = inputPort1;
                    outputPort = outputPort1;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                var connection = new Connection(outputPort, inputPort);
                _connections.Add(connection);
            }

            var gateModels = _logicModelsMap.Keys.ToList();

            return new LogicScheme(gateModels, gateModels.OfType<InputGate>().ToList(), gateModels.OfType<OutputGate>().ToList(), [.. _connections], _logicModelsMap.ToDictionary());
        }
        catch (Exception e)
        {
            _logger.LogError("Error occupied while building scheme:\n{e}", e);
            throw;
        }
    }

    public void CreateLogicModelFrom(InputGateViewModel gate)
    {
        var logicModel = new InputGate { Delay = gate.Delay };
        var link = new GateLogicModelToViewModelLink(logicModel, gate, new Dictionary<BasePort, PortLogicModelToViewModelLink>
        {
            [logicModel.Output] = new(logicModel.Output, gate.OutputPort)
        });
        _logicModelsMap[logicModel] = link;
    }

    public void CreateLogicModelFrom(OutputGateViewModel gate)
    {
        var logicModel = new OutputGate { Delay = gate.Delay };
        var link = new GateLogicModelToViewModelLink(logicModel, gate, new Dictionary<BasePort, PortLogicModelToViewModelLink>
        {
            [logicModel.Input] = new(logicModel.Input, gate.InputPort)
        });
        _logicModelsMap[logicModel] = link;
    }

    public void CreateLogicModelFrom(AndGateViewModel gate)
    {
        var logicModel = new AndGate { InputPortsCount = gate.InputPorts.Count(), Delay = gate.Delay };
        var link = new GateLogicModelToViewModelLink(logicModel, gate, []);
        var inputPorts = gate.InputPorts.ToList();

        for (var i = 0; i < inputPorts.Count; i++)
        {
            link.PortsMap[logicModel.Inputs[i]] = new PortLogicModelToViewModelLink(logicModel.Inputs[i], inputPorts[i]);
        }

        link.PortsMap[logicModel.Output] = new PortLogicModelToViewModelLink(logicModel.Output, gate.OutputPort);

        _logicModelsMap[logicModel] = link;
    }
}
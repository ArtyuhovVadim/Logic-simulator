using LogicSimulator.Core;
using LogicSimulator.Core.Gates;
using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.Logic.Gates;
using LogicSimulator.Models.SchemeGraph;
using LogicSimulator.Models.SchemeGraph.Base;

namespace LogicSimulator.Models.Logic;

public class LogicScheme
{
    private readonly IPreprocessedLogicScheme _preprocessedLogicScheme;
    private readonly List<SchemeValidationResult> _validationResults;
    private readonly List<Connection> _connections = [];

    public LogicScheme(IPreprocessedLogicScheme preprocessedLogicScheme, IEnumerable<SchemeValidationResult> validationResults)
    {
        _preprocessedLogicScheme = preprocessedLogicScheme;
        _validationResults = [.. validationResults];
        IsValid = false;
    }

    public LogicScheme(IPreprocessedLogicScheme preprocessedLogicScheme, List<Connection> connections, IReadOnlyList<SchemeValidationResult> validationResults)
    {
        _preprocessedLogicScheme = preprocessedLogicScheme;
        _connections = [.. connections];
        _validationResults = [.. validationResults];
        IsValid = true;
    }

    public bool IsValid { get; }

    public IReadOnlyList<SchemeValidationResult> ValidationResults => _validationResults;

    public IEnumerable<ISchemeGraphGateNode> GateNodes => _preprocessedLogicScheme.Nodes;

    public IEnumerable<ISchemeGraphGateNode<InputGate, InputGateModel>> InputGateNodes => _preprocessedLogicScheme.Nodes.Where(x => x.LogicModel is InputGate).Select(x => new SchemeGraphGateNode<InputGate, InputGateModel>(x));

    public IEnumerable<ISchemeGraphGateNode<OutputGate, OutputGateModel>> OutputGateNodes => _preprocessedLogicScheme.Nodes.Where(x => x.LogicModel is OutputGate).Select(x => new SchemeGraphGateNode<OutputGate, OutputGateModel>(x));

    public IEnumerable<Connection> Connections => _connections;

    public IEnumerable<InputGate> InputGateLogicModels => GateNodes.Select(x => x.LogicModel).OfType<InputGate>();

    public IEnumerable<OutputGate> OutputGateLogicModels => GateNodes.Select(x => x.LogicModel).OfType<OutputGate>();
}
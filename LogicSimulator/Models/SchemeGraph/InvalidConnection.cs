using LogicSimulator.Models.SchemeGraph.Base;

namespace LogicSimulator.Models.SchemeGraph;

public record InvalidConnection(IReadOnlyList<ISchemeGraphPortNode> InputPorts, IReadOnlyList<ISchemeGraphPortNode> OutputPorts, IWiresGroup WiresGroup);
namespace LogicSimulator.Models.SchemeGraph.Base;

public interface ISchemeGraphEdge
{
    ISchemeGraphPortNode Source { get; }

    ISchemeGraphPortNode Recipient { get; }

    IWiresGroup WiresGroup { get; }
}
using System.Diagnostics;
using LogicSimulator.Models.SchemeGraph.Base;

namespace LogicSimulator.Models.SchemeGraph;

[DebuggerDisplay("{Source}->{Recipient}")]
public class SchemeGraphEdge : ISchemeGraphEdge
{
    public SchemeGraphEdge(SchemeGraphPortNode source, SchemeGraphPortNode recipient, WiresGroup wiresGroup)
    {
        Source = source;
        Recipient = recipient;
        WiresGroup = wiresGroup;
    }

    public SchemeGraphPortNode Source { get; }

    public SchemeGraphPortNode Recipient { get; }

    public WiresGroup WiresGroup { get; }

    ISchemeGraphPortNode ISchemeGraphEdge.Source => Source;

    ISchemeGraphPortNode ISchemeGraphEdge.Recipient => Recipient;

    IWiresGroup ISchemeGraphEdge.WiresGroup => WiresGroup;
}
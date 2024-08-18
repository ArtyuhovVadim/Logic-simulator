using LogicSimulator.Models.Logic;
using LogicSimulator.ViewModels.Anchorable;

namespace LogicSimulator.Models.MessageSources;

public class WiresMessageSource : IMessageSource
{
    private readonly SchemeViewModel _scheme;
    private readonly IEnumerable<WireModel> _wires;

    public WiresMessageSource(SchemeViewModel scheme, IEnumerable<WireModel> wires)
    {
        _scheme = scheme;
        _wires = wires;
    }

    public string Name => "Группа проводников";

    public void GoTo() => _scheme.PanToObjectsAndSelect(_wires.ToArray());
}
using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Shared.ExtensionMethods;
using LogicSimulator.ViewModels.Anchorable;

namespace LogicSimulator.Models.MessageSources;

public class GateMessageSource : IMessageSource
{
    private readonly SchemeViewModel _scheme;
    private readonly BaseGateModel _gate;

    public GateMessageSource(SchemeViewModel scheme, BaseGateModel gate)
    {
        _scheme = scheme;
        _gate = gate;
    }

    public string Name => $"Вентиль{_gate.Name.ReturnIfNotEmpty()}с позицией ({_gate.Location})";

    public void GoTo() => _scheme.PanToObjectAndSelect(_gate);
}
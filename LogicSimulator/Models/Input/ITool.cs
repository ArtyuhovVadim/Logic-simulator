using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.Tools;

namespace LogicSimulator.Models.Input;

public interface ITool : IInputTarget
{
    string Name { get; }

    ToolGroup Group { get; }

    void Activate(IToolSwitcherService toolSwitcher, bool activatedFromOtherTool);

    void Deactivate();
}
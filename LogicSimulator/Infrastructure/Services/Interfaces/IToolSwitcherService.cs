using LogicSimulator.Models.Input;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IToolSwitcherService
{
    event ToolChanged? ToolChanged;

    event Action? ToolLocked;

    IEnumerable<ITool> Tools { get; }

    ITool? CurrentTool { get; }

    ITool? DefaultTool { get; set; }

    bool IsCurrentToolLocked { get; set; }

    void AddTool(ITool tool);

    void AddTools(params ITool[] tools);

    void SwitchToEmptyTool();

    bool SwitchTool(ITool? tool, bool isActivatedFromAnotherTool, Action<ITool>? actionToNextToolAfterActivating = null);

    bool SwitchTool(Type toolType, bool isActivatedFromAnotherTool, Action<ITool>? actionToNextToolAfterActivating = null);

    bool SwitchTool<T>(bool isActivatedFromAnotherTool, Action<T>? actionToNextToolAfterActivating = null) where T : ITool;

    bool SwitchToDefaultTool(bool isActivatedFromAnotherTool);
}
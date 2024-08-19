using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Input;

namespace LogicSimulator.Infrastructure.Services;

public class ToolSwitcherService : IToolSwitcherService
{
    private readonly Dictionary<Type, ITool> _toolsMap = [];

    public event ToolChanged? ToolChanged;

    public IEnumerable<ITool> Tools => _toolsMap.Values;

    public ITool? CurrentTool { get; private set; }

    public ITool? DefaultTool { get; set; }

    public bool IsCurrentToolLocked { get; set; }

    public void AddTool(ITool tool)
    {
        var toolType = tool.GetType();

        if (!_toolsMap.TryAdd(toolType, tool))
            throw new InvalidOperationException($"{toolType.Name} has already been added.");
    }

    public void AddTools(params ITool[] tools)
    {
        foreach (var tool in tools)
        {
            AddTool(tool);
        }
    }

    public bool SwitchTool<T>(bool isActivatedFromAnotherTool, Action<T>? actionToNextToolAfterActivating = null) where T : ITool
    {
        var toolType = typeof(T);

        if (!_toolsMap.TryGetValue(toolType, out var nextTool))
            throw new InvalidOperationException($"{toolType.Name} is not present in tools collection.");

        if (nextTool == CurrentTool)
            return false;

        var res = ChangeTool(nextTool, isActivatedFromAnotherTool);
        if (res)
            actionToNextToolAfterActivating?.Invoke((T)nextTool);
        return res;
    }

    public bool SwitchTool(Type toolType, bool isActivatedFromAnotherTool, Action<ITool>? actionToNextToolAfterActivating = null)
    {
        if (!_toolsMap.TryGetValue(toolType, out var nextTool))
            throw new InvalidOperationException($"{toolType.Name} is not present in tools collection.");

        if (nextTool == CurrentTool)
            return false;

        var res = ChangeTool(nextTool, isActivatedFromAnotherTool);
        if(res)
            actionToNextToolAfterActivating?.Invoke(nextTool);
        return res;
    }

    public bool SwitchToDefaultTool(bool isActivatedFromAnotherTool) => ChangeTool(DefaultTool, isActivatedFromAnotherTool);

    private bool ChangeTool(ITool? newTool, bool isActivatedFromAnotherTool)
    {
        if (IsCurrentToolLocked)
            return false;

        var oldTool = CurrentTool;

        oldTool?.Deactivate();
        newTool?.Activate(this, isActivatedFromAnotherTool);

        CurrentTool = newTool;
        ToolChanged?.Invoke(oldTool, DefaultTool);

        return true;
    }
}
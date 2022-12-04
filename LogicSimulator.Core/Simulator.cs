using LogicSimulator.Core.LogicComponents.Base;

namespace LogicSimulator.Core;

public class Simulator
{
    private readonly IEnumerable<LogicComponent> _components;

    private readonly List<LogicComponent> _dirtyComponents;

    public Simulator(IEnumerable<LogicComponent> components)
    {
        _components = components;
        _dirtyComponents = components.ToList();
    }

    public void Simulate()
    {
        while (_dirtyComponents.Count != 0)
        {
            var firstDirty = _dirtyComponents.FirstOrDefault(x => x.IsDirty);

            if (firstDirty is null) break;

            firstDirty.Update();
        }
    }
}
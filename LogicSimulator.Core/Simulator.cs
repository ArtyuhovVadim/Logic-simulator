using LogicSimulator.Core.Gates;
using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Core;

public delegate void SimulationEventExecutedHandler(Simulator simulator, BaseGate gate, BasePort port);

public delegate void SimulationStepExecutedHandler(Simulator simulator);

public delegate void PortStateChangedHandler(Simulator simulator, BaseGate gate, BasePort port, SignalType oldState, SignalType newState);

public class Simulator
{
    private readonly Dictionary<ulong, Queue<SimulationEvent>> _eventsMap = [];

    public event SimulationEventExecutedHandler? SimulationEventExecuted;

    public event SimulationStepExecutedHandler? SimulationStepExecuted;

    public event PortStateChangedHandler? PortStateChanged;

    public int EventsCount => _eventsMap.Count;

    public ulong CurrentTime { get; private set; }

    public void Reset()
    {
        CurrentTime = 0;
        _eventsMap.Clear();
    }

    public void InvalidateInputs(IEnumerable<InputGate> inputs)
    {
        foreach (var input in inputs)
            input.Invalidate(this);
    }

    public void Simulate(IEnumerable<InputGate> inputs, ulong maxTime = ulong.MaxValue)
    {
        InvalidateInputs(inputs);
        while (_eventsMap.Count > 0 && CurrentTime < maxTime)
            SimulateStep();
    }

    public void OnPortStateChanged(BasePort port, SignalType oldState, SignalType newState) =>
        PortStateChanged?.Invoke(this, port.Parent, port, oldState, newState);

    public void PushEvent(BasePort port, SignalType newState, ulong duration)
    {
        if (!_eventsMap.ContainsKey(CurrentTime + duration))
            _eventsMap[CurrentTime + duration] = new Queue<SimulationEvent>();

        var newEvent = new SimulationEvent(port, newState, CurrentTime, duration);
        var queue = _eventsMap[CurrentTime + duration];

        while (queue.Count > 0 && queue.Peek().ExecutionTime == newEvent.ExecutionTime && queue.Peek().Port == newEvent.Port)
            queue.Dequeue();

        queue.Enqueue(newEvent);
    }

    public void SimulateStep()
    {
        if (!_eventsMap.TryGetValue(CurrentTime, out var queue))
        {
            SimulationStepExecuted?.Invoke(this);
            CurrentTime++;
            return;
        }

        while (queue.Count > 0)
        {
            var simulationEvent = queue.Dequeue();
            simulationEvent.Execute(this);
            SimulationEventExecuted?.Invoke(this, simulationEvent.Port.Parent, simulationEvent.Port);
        }

        _eventsMap.Remove(CurrentTime);
        SimulationStepExecuted?.Invoke(this);
        CurrentTime++;
    }

    private record SimulationEvent(BasePort Port, SignalType NewState, ulong RaiseTime, ulong Delay)
    {
        public ulong ExecutionTime => RaiseTime + Delay;

        public void Execute(Simulator simulator) => Port.Invalidate(simulator, NewState);
    }
}
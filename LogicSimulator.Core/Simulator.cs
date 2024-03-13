using LogicSimulator.Core.Gates;

namespace LogicSimulator.Core;

public class Simulator
{
    private readonly Dictionary<long, Queue<SimulationEvent>> _eventsMap = [];

    public long CurrentTime { get; private set; }

    public void Simulate(IEnumerable<InputGate> inputs)
    {
        foreach (var input in inputs)
            input.Invalidate();

        while (_eventsMap.Count > 0)
            SimulateStep();
    }

    public void PushEvent(Port port, SignalType newState, long duration)
    {
        if (!_eventsMap.ContainsKey(CurrentTime + duration))
            _eventsMap[CurrentTime + duration] = new Queue<SimulationEvent>();

        var newEvent = new SimulationEvent(port, newState, CurrentTime, duration);
        var queue = _eventsMap[CurrentTime + duration];

        while (queue.Count > 0 && queue.Peek().ExecutionTime == newEvent.ExecutionTime && queue.Peek().Port == newEvent.Port)
            queue.Dequeue();

        queue.Enqueue(newEvent);
    }

    private void SimulateStep()
    {
        if (!_eventsMap.TryGetValue(CurrentTime, out var queue))
        {
            CurrentTime++;
            return;
        }

        while (queue.Count > 0)
            queue.Dequeue().Execute();

        _eventsMap.Remove(CurrentTime);
        CurrentTime++;
    }

    private record SimulationEvent(Port Port, SignalType NewState, long RaiseTime, long Delay)
    {
        public long ExecutionTime => RaiseTime + Delay;

        public void Execute() => Port.State = NewState;
    }
}
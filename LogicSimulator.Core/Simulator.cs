using LogicSimulator.Core.Gates;

namespace LogicSimulator.Core;

public class Simulator
{
    private readonly Dictionary<ulong, Queue<SimulationEvent>> _eventsMap = [];

    public ulong CurrentTime { get; private set; }

    public void Simulate(IEnumerable<InputGate> inputs, ulong maxTime = ulong.MaxValue)
    {
        foreach (var input in inputs)
            input.Invalidate(this);

        while (_eventsMap.Count > 0 && CurrentTime < maxTime)
            SimulateStep();
    }

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

    private void SimulateStep()
    {
        if (!_eventsMap.TryGetValue(CurrentTime, out var queue))
        {
            CurrentTime++;
            return;
        }

        while (queue.Count > 0)
            queue.Dequeue().Execute(this);

        _eventsMap.Remove(CurrentTime);
        CurrentTime++;
    }

    private record SimulationEvent(BasePort Port, SignalType NewState, ulong RaiseTime, ulong Delay)
    {
        public ulong ExecutionTime => RaiseTime + Delay;

        public void Execute(Simulator simulator) => Port.Invalidate(simulator, NewState);
    }
}
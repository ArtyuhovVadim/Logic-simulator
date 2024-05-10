using System.Diagnostics;
using LogicSimulator.Core;

namespace LogicSimulator.Infrastructure.Services;

public enum SimulationState
{
    Started,
    Paused,
    Stopped
}

public struct SimulatorSettings()
{
    public ulong MaxTime = ulong.MaxValue;

    public bool IsPauseSupported = true;

    public bool StepByStepOnStart = false;
}

public class SchemeSimulatorService
{
    private readonly Simulator _simulator = new();
    private LogicScheme? _scheme;
    private SimulatorSettings _settings;

    private CancellationTokenSource _cancellationTokenSource = new();
    private readonly AutoResetEvent _simulationAutoResetEvent = new(false);
    private readonly AutoResetEvent _simulationStepAutoResetEvent = new(false);
    private Task? _simulationTask;

    public SchemeSimulatorService()
    {
        _simulator.SimulationStepExecuted += OnSimulationStepExecuted;
    }

    public SimulatorSettings Settings => _settings;

    public SimulationState State { get; private set; } = SimulationState.Stopped;

    public bool CanStart => State is SimulationState.Stopped;

    public bool CanResume => State is SimulationState.Paused;

    public bool CanPause => State is SimulationState.Started && _settings.IsPauseSupported;

    public bool CanStop => State is SimulationState.Started or SimulationState.Paused;

    public bool CanSimulateNextStep => State is SimulationState.Paused;

    public void StartSimulation(LogicScheme scheme, SimulatorSettings settings)
    {
        if (!CanStart)
            return;

        _scheme = scheme;
        _settings = settings;

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        if (!settings.StepByStepOnStart)
            _simulationStepAutoResetEvent.Set();

        _simulationTask = Task.Run(() =>
        {
            try
            {
                _simulator.Reset();
                _simulator.InvalidateInputs(scheme.InputGates);

                while (!token.IsCancellationRequested)
                {
                    while (_simulator.EventsCount > 0 && _simulator.CurrentTime < _settings.MaxTime)
                    {
                        _simulationStepAutoResetEvent.WaitOne();
                        token.ThrowIfCancellationRequested();
                        _simulator.SimulateStep();
                        //TODO:   if (!_settings.IsPauseSupported) continue;
                    }

                    token.ThrowIfCancellationRequested();

                    Debug.WriteLine($"{Task.CurrentId} - Waiting...");
                    _simulationAutoResetEvent.WaitOne();
                }
            }
            catch (OperationCanceledException) { Debug.WriteLine("Simulation cancelled."); }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }, token);

        State = settings.StepByStepOnStart ? SimulationState.Paused : SimulationState.Started;
    }

    public void SimulateNextStep()
    {
        if (!CanSimulateNextStep)
            return;

        _simulationStepAutoResetEvent.Set();
        _simulationStepAutoResetEvent.Set();
    }

    public void ResumeSimulation()
    {
        if (!CanResume)
            return;

        _simulationAutoResetEvent.Set();
        _simulationStepAutoResetEvent.Set();

        State = SimulationState.Started;
    }

    public void PauseSimulation()
    {
        if (!CanPause)
            return;

        _simulationStepAutoResetEvent.Reset();
        _simulationAutoResetEvent.Reset();

        State = SimulationState.Paused;
    }

    public void StopSimulation()
    {
        if (!CanStop)
            return;

        _cancellationTokenSource.Cancel();
        _simulationAutoResetEvent.Set();
        _simulationStepAutoResetEvent.Set();
        _simulationTask!.Wait();
        _simulationAutoResetEvent.Reset();
        _simulationStepAutoResetEvent.Reset();
        State = SimulationState.Stopped;
    }

    private void OnSimulationStepExecuted(Simulator simulator)
    {
        if (State is SimulationState.Started)
        {
            _simulationStepAutoResetEvent.Set();
        }
        else if (State is SimulationState.Paused)
        {
            _simulationStepAutoResetEvent.Reset();
        }

        Debug.WriteLine($"Step: {simulator.CurrentTime}|In: {string.Join(' ', _scheme!.InputGates.Select(x => x.State))}|Out: {string.Join(' ', _scheme.OutputGates.Select(x => x.Input.State))}");
    }
}
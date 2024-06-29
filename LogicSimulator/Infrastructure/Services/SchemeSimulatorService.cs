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

    public bool IsPausedOnStart = false;

    public ulong AdditionalSimulationTime = 0;
}

//TODO: Handle exceptions
public class SchemeSimulatorService
{
    private readonly Simulator _simulator = new();
    private LogicScheme? _scheme;
    private SimulatorSettings _settings;
    private ulong _additionalSimulationTime;

    private CancellationTokenSource _cancellationTokenSource = null!;
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

        if (!settings.IsPausedOnStart)
            _simulationStepAutoResetEvent.Set();

        _additionalSimulationTime = settings.AdditionalSimulationTime;

        _simulationTask = Task.Run(() =>
        {
            try
            {
                _simulator.Reset();
                _simulator.InvalidateInputs(scheme.InputGates);

                while (!token.IsCancellationRequested)
                {
                    var sw = Stopwatch.StartNew();

                    while ((_simulator.EventsCount > 0 || _additionalSimulationTime > 0) && _simulator.CurrentTime < _settings.MaxTime)
                    {
                        if (_settings.IsPauseSupported)
                            _simulationStepAutoResetEvent.WaitOne();

                        token.ThrowIfCancellationRequested();
                        _simulator.SimulateStep();

                        if (_simulator.EventsCount == 0)
                            _additionalSimulationTime--;
                    }

                    token.ThrowIfCancellationRequested();
                    sw.Stop();
                    Debug.WriteLine($"{Task.CurrentId} - Waiting... {sw.Elapsed.TotalMilliseconds}");
                    _simulationAutoResetEvent.WaitOne();
                }
            }
            catch (OperationCanceledException) { Debug.WriteLine("Simulation cancelled."); }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }, token);

        State = settings.IsPausedOnStart ? SimulationState.Paused : SimulationState.Started;
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
        if (_settings.IsPauseSupported)
        {
            if (State is SimulationState.Started)
            {
                _simulationStepAutoResetEvent.Set();
            }
            else if (State is SimulationState.Paused)
            {
                _simulationStepAutoResetEvent.Reset();
            }
        }

        //Debug.WriteLine($"Step: {simulator.CurrentTime}|In: {string.Join(' ', _scheme!.InputGates.Select(x => x.State))}|Out: {string.Join(' ', _scheme.OutputGates.Select(x => x.Input.State))}");
    }
}
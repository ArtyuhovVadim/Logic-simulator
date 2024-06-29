using LogicSimulator.Core;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using Microsoft.Extensions.Logging;

namespace LogicSimulator.Infrastructure.Services;

public class SchemeSimulatorService : ISchemeSimulatorService
{
    private readonly ILogger<SchemeSimulatorService> _logger;
    private readonly Simulator _simulator = new();
    private LogicScheme? _scheme;
    private SimulatorSettings _settings;
    private ulong _additionalSimulationTime;

    private CancellationTokenSource _cancellationTokenSource = null!;
    private readonly AutoResetEvent _simulationAutoResetEvent = new(false);
    private readonly AutoResetEvent _simulationStepAutoResetEvent = new(false);
    private Task? _simulationTask;

    private Dictionary<string, PortSimulationResult> _simulationResult = null!;

    public SchemeSimulatorService(ILogger<SchemeSimulatorService> logger)
    {
        _logger = logger;
        _simulator.SimulationStepExecuted += OnSimulationStepExecuted;
    }

    public IReadOnlyDictionary<string, PortSimulationResult> Result => _simulationResult;

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

        _logger.LogInformation("Simulation started");

        _scheme = scheme;
        _settings = settings;

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        if (!settings.IsPausedOnStart)
            _simulationStepAutoResetEvent.Set();

        _additionalSimulationTime = settings.AdditionalSimulationTime;
        _simulationResult = [];

        _simulationTask = Task.Run(() =>
        {
            try
            {
                _simulator.Reset();
                _simulator.InvalidateInputs(scheme.InputGates);

                while (!token.IsCancellationRequested)
                {
                    while ((_simulator.EventsCount > 0 || _additionalSimulationTime > 0) &&
                           _simulator.CurrentTime < _settings.MaxTime)
                    {
                        if (_settings.IsPauseSupported)
                            _simulationStepAutoResetEvent.WaitOne();

                        token.ThrowIfCancellationRequested();
                        _simulator.SimulateStep();

                        if (_simulator.EventsCount == 0)
                            _additionalSimulationTime--;
                    }

                    token.ThrowIfCancellationRequested();
                    _logger.LogInformation("Simulation thread is waiting...");
                    _simulationAutoResetEvent.WaitOne();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Simulation cancelled");
            }
            catch (Exception e)
            {
                _logger.LogError("Simulation error:\n{e}", e);
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

        _logger.LogInformation("Simulation next step has been executed");
    }

    public void ResumeSimulation()
    {
        if (!CanResume)
            return;

        _simulationAutoResetEvent.Set();
        _simulationStepAutoResetEvent.Set();

        State = SimulationState.Started;

        _logger.LogInformation("Simulation has been resumed");
    }

    public void PauseSimulation()
    {
        if (!CanPause)
            return;

        _simulationStepAutoResetEvent.Reset();
        _simulationAutoResetEvent.Reset();

        State = SimulationState.Paused;

        _logger.LogInformation("Simulation has been paused");
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

        foreach (var inputGate in _scheme!.InputGates)
        {
            var inputGateViewModel = _scheme.GatesMap[inputGate].ViewModel;
            _simulationResult[inputGateViewModel.Name].States.Add(new PortState(_simulator.CurrentTime, inputGate.Output.State));
        }

        foreach (var outputGate in _scheme.OutputGates)
        {
            var outputGateViewModel = _scheme.GatesMap[outputGate].ViewModel;
            _simulationResult[outputGateViewModel.Name].States.Add(new PortState(_simulator.CurrentTime, outputGate.Input.State));
        }

        _logger.LogInformation("Simulation has been stopped");
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

        foreach (var inputGate in _scheme!.InputGates)
        {
            var inputGateViewModel = _scheme.GatesMap[inputGate].ViewModel;

            if (!_simulationResult.TryGetValue(inputGateViewModel.Name, out var result))
            {
                result = new PortSimulationResult(inputGateViewModel.Name);
                _simulationResult[inputGateViewModel.Name] = result;
            }

            if (result.States.Count == 0 || result.States.Last().State != inputGate.Output.State)
                result.States.Add(new PortState(_simulator.CurrentTime, inputGate.Output.State));
        }

        foreach (var outputGate in _scheme.OutputGates)
        {
            var outputGateViewModel = _scheme.GatesMap[outputGate].ViewModel;

            if (!_simulationResult.TryGetValue(outputGateViewModel.Name, out var result))
            {
                result = new PortSimulationResult(outputGateViewModel.Name);
                _simulationResult[outputGateViewModel.Name] = result;
            }

            if (result.States.Count == 0 || result.States.Last().State != outputGate.Input.State)
                result.States.Add(new PortState(_simulator.CurrentTime, outputGate.Input.State));
        }
    }
}
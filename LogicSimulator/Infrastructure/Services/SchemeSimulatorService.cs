using LogicSimulator.Core;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Logic;
using LogicSimulator.Models.Simulation;
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
    private readonly ManualResetEventSlim _simulationResetEvent = new(false);
    private readonly ManualResetEventSlim _simulationStepResetEvent = new(false);
    private readonly ReaderWriterLockSlim _lockSlim = new();
    private Task? _simulationTask;

    private Dictionary<string, PortSimulationResult> _simulationResult = [];
    private SimulationState _state = SimulationState.Stopped;

    public SchemeSimulatorService(ILogger<SchemeSimulatorService> logger)
    {
        _logger = logger;
        _simulator.SimulationStepExecuted += OnSimulationStepExecuted;
    }

    public event SimulationStateChanged? SimulationStateChanged;

    public IReadOnlyDictionary<string, PortSimulationResult> Result => _simulationResult;

    public SimulatorSettings Settings => _settings;

    public SimulationState State
    {
        get => _state;
        private set
        {
            if (_state == value) return;
            var oldState = _state;
            _state = value;
            SimulationStateChanged?.Invoke(oldState, value);
        }
    }

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
            _simulationStepResetEvent.Set();

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
                            _simulationStepResetEvent.Wait(token);

                        token.ThrowIfCancellationRequested();
                        _simulator.SimulateStep();

                        if (_simulator.EventsCount == 0)
                            _additionalSimulationTime--;
                    }

                    token.ThrowIfCancellationRequested();
                    _logger.LogInformation("Simulation thread is waiting... (CurrentTime: {CurrentTime})", _simulator.CurrentTime);
                    _simulationResetEvent.Wait(token);
                    _simulationResetEvent.Reset();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Simulation cancelled (CurrentTime: {CurrentTime})", _simulator.CurrentTime);
            }
            catch (Exception e)
            {
                _logger.LogError("Simulation error (CurrentTime: {CurrentTime}):\n{e}", _simulator.CurrentTime, e);
                throw;
            }
        }, token);

        State = settings.IsPausedOnStart ? SimulationState.Paused : SimulationState.Started;
    }

    public void SimulateNextStep()
    {
        if (!CanSimulateNextStep)
            return;

        _logger.LogInformation("Simulation next step has been executed (CurrentTime: {CurrentTime})", _simulator.CurrentTime);

        _simulationStepResetEvent.Set();
        _simulationStepResetEvent.Set();

        AddCurrentStatesToResult();
    }

    public void ResumeSimulation()
    {
        if (!CanResume)
            return;

        _logger.LogInformation("Simulation has been resumed (CurrentTime: {CurrentTime})", _simulator.CurrentTime);

        _simulationResetEvent.Set();
        _simulationStepResetEvent.Set();

        State = SimulationState.Started;
    }

    public void PauseSimulation()
    {
        if (!CanPause)
            return;

        _logger.LogInformation("Simulation has been paused (CurrentTime: {CurrentTime})", _simulator.CurrentTime);

        _simulationStepResetEvent.Reset();
        _simulationResetEvent.Reset();
        AddCurrentStatesToResult();

        State = SimulationState.Paused;
    }

    public void StopSimulation()
    {
        if (!CanStop)
            return;

        _logger.LogInformation("Simulation has been stopped (CurrentTime: {CurrentTime})", _simulator.CurrentTime);

        _cancellationTokenSource.Cancel();
        _simulationResetEvent.Set();
        _simulationStepResetEvent.Set();
        _simulationTask!.Wait();
        _simulationResetEvent.Reset();
        _simulationStepResetEvent.Reset();
        AddCurrentStatesToResult();

        State = SimulationState.Stopped;
    }

    public Dictionary<string, PortSimulationResult> GetSimulationResult()
    {
        _lockSlim.EnterReadLock();
        try
        {
            return _simulationResult.ToDictionary();
        }
        finally
        {
            _lockSlim.ExitReadLock();
        }
    }

    private void AddCurrentStatesToResultIfNotPresent()
    {
        _lockSlim.EnterWriteLock();
        try
        {
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
        finally
        {
            _lockSlim.ExitWriteLock();
        }
    }

    private void AddCurrentStatesToResult()
    {
        _lockSlim.EnterWriteLock();
        try
        {
            foreach (var inputGate in _scheme!.InputGates)
            {
                var inputGateViewModel = _scheme.GatesMap[inputGate].ViewModel;

                if (!_simulationResult.TryGetValue(inputGateViewModel.Name, out var result))
                {
                    result = new PortSimulationResult(inputGateViewModel.Name);
                    _simulationResult[inputGateViewModel.Name] = result;
                }

                var newState = new PortState(_simulator.CurrentTime, inputGate.Output.State);

                if (result.States.Count > 1 && result.States[^2].State == inputGate.Output.State)
                    result.States[^1] = newState;
                else
                    result.States.Add(newState);
            }

            foreach (var outputGate in _scheme.OutputGates)
            {
                var outputGateViewModel = _scheme.GatesMap[outputGate].ViewModel;

                if (!_simulationResult.TryGetValue(outputGateViewModel.Name, out var result))
                {
                    result = new PortSimulationResult(outputGateViewModel.Name);
                    _simulationResult[outputGateViewModel.Name] = result;
                }

                var newState = new PortState(_simulator.CurrentTime, outputGate.Input.State);

                if (result.States.Count > 1 && result.States[^2].State == outputGate.Input.State)
                    result.States[^1] = newState;
                else
                    result.States.Add(newState);
            }
        }
        finally
        {
            _lockSlim.ExitWriteLock();
        }
    }

    private void OnSimulationStepExecuted(Simulator simulator)
    {
        if (_settings.IsPauseSupported && State is SimulationState.Paused)
        {
            _simulationStepResetEvent.Reset();
        }

        AddCurrentStatesToResultIfNotPresent();
    }
}
using LogicSimulator.Core;
using LogicSimulator.Shared;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class GateStateViewModel : BindableBase, IWaveState
{
    #region Time

    private ulong _time;

    public ulong Time
    {
        get => _time;
        set => Set(ref _time, value);
    }

    #endregion

    #region State

    private SignalType _state = SignalType.Undefined;

    public SignalType State
    {
        get => _state;
        set => Set(ref _state, value);
    }

    #endregion
}
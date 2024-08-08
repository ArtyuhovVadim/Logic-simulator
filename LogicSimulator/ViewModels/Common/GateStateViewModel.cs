using LogicSimulator.Core;
using LogicSimulator.Models.Logic;
using LogicSimulator.Shared.Models;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.Common;

public class GateStateViewModel : BindableBase, IWaveState
{
    public GateStateViewModel(PortState model)
    {
        Time = model.Time;
        State = model.State;
    }

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
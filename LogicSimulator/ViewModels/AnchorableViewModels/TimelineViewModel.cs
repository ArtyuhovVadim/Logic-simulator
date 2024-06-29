using LogicSimulator.Core;
using LogicSimulator.Shared;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.AnchorableViewModels;

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

public class TimelineRowViewModel : BindableBase, IWave
{
    #region Label

    private string _label = string.Empty;

    public string Label
    {
        get => _label;
        set => Set(ref _label, value);
    }

    #endregion

    #region Scale

    private double _scale = 1;

    public double Scale
    {
        get => _scale;
        set => Set(ref _scale, value);
    }

    #endregion

    #region Offset

    private double _offset;

    public double Offset
    {
        get => _offset;
        set => Set(ref _offset, value);
    }

    #endregion

    #region States

    private ObservableCollection<GateStateViewModel> _states = [];

    public ObservableCollection<GateStateViewModel> States
    {
        get => _states;
        set => Set(ref _states, new ObservableCollection<GateStateViewModel>(value));
    }

    #endregion

    IEnumerable<IWaveState> IWave.States => States;
}

public class TimelineViewModel : ToolViewModel
{
    public override string Title => "Таймлайн";

    #region HorizontalOffset

    private double _horizontalOffset;

    public double HorizontalOffset
    {
        get => _horizontalOffset;
        set
        {
            if (Set(ref _horizontalOffset, value))
            {
                InvalidateWavesScaleAndOffset();
            }
        }
    }

    #endregion

    #region VerticalOffset

    private double _verticalOffset;

    public double VerticalOffset
    {
        get => _verticalOffset;
        set => Set(ref _verticalOffset, value);
    }

    #endregion

    #region Scale

    private double _scale = 1;

    public double Scale
    {
        get => _scale;
        set
        {
            if (Set(ref _scale, value))
            {
                AlternationCount = Math.Max(10, (int)(10 / Scale / 5) * 10);
                InvalidateWavesScaleAndOffset();
            }
        }
    }

    #endregion

    #region Frequency

    private int _frequency = 10;

    public int Frequency
    {
        get => _frequency;
        set => Set(ref _frequency, value);
    }

    #endregion

    #region AlternationCount

    private int _alternationCount = 10;

    public int AlternationCount
    {
        get => _alternationCount;
        set => Set(ref _alternationCount, value);
    }

    #endregion

    #region TimeUnitSuffix

    private string _timeUnitSuffix = "ns";

    public string TimeUnitSuffix
    {
        get => _timeUnitSuffix;
        set => Set(ref _timeUnitSuffix, value);
    }

    #endregion

    #region SignalHeight

    private double _signalHeight = 30;

    public double SignalHeight
    {
        get => _signalHeight;
        set => Set(ref _signalHeight, value);
    }

    #endregion

    #region MaxSignalsTime

    private ulong _maxSignalsTime;

    public ulong MaxSignalsTime
    {
        get => _maxSignalsTime;
        private set => Set(ref _maxSignalsTime, value);
    }

    #endregion

    #region WavesCount

    public int WavesCount => _waves.Count;

    #endregion

    #region Waves

    private ObservableCollection<TimelineRowViewModel> _waves = [];

    public IEnumerable<TimelineRowViewModel> Waves
    {
        get => _waves;
        set
        {
            if (Set(ref _waves, new ObservableCollection<TimelineRowViewModel>(value)))
            {
                MaxSignalsTime = value.Max(x => x.States.Last().Time);
                OnPropertyChanged(nameof(WavesCount));
                InvalidateWavesScaleAndOffset();
            }
        }
    }

    #endregion

    private void InvalidateWavesScaleAndOffset()
    {
        foreach (var model in Waves)
        {
            model.Scale = Scale;
            model.Offset = HorizontalOffset;
        }
    }
}
using LogicSimulator.Infrastructure.Messages;
using LogicSimulator.ViewModels.Anchorable.Base;
using LogicSimulator.ViewModels.Common;
using WpfExtensions.Mvvm.Messaging;

namespace LogicSimulator.ViewModels.Anchorable;

public class TimelineViewModel :
    ToolViewModel,
    IRecipient<SimulationStateChangedMessage>,
    IRecipient<DocumentActivatedMessage>,
    IRecipient<DocumentDeactivatedMessage>
{
    private readonly IMessageBus _messageBus;

    public TimelineViewModel(IMessageBus messageBus)
    {
        _messageBus = messageBus;

        _messageBus.RegisterHandler<SimulationStateChangedMessage>(this);
        _messageBus.RegisterHandler<DocumentActivatedMessage>(this);
        _messageBus.RegisterHandler<DocumentDeactivatedMessage>(this);
    }

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

    public ObservableCollection<TimelineRowViewModel> Waves
    {
        get => _waves;
        set
        {
            if (Set(ref _waves, value))
            {
                if (value.Any() && value.All(x => x.States.Count > 0))
                    MaxSignalsTime = value.Max(x => x.States.Last().Time);
                else
                    MaxSignalsTime = 0;
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

    public void Receive(SimulationStateChangedMessage message)
    {
        Waves = new ObservableCollection<TimelineRowViewModel>(message.SimulationResult.Select(x => new TimelineRowViewModel(x.Value)));
    }

    public void Receive(DocumentActivatedMessage message)
    {
        if (message.Document is not SchemeViewModel scheme) return;

        Waves = new ObservableCollection<TimelineRowViewModel>(scheme.GetSimulationResult().Select(x => new TimelineRowViewModel(x.Value)));
    }

    public void Receive(DocumentDeactivatedMessage message)
    {
        Waves = [];
    }
}
using LogicSimulator.Shared;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

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
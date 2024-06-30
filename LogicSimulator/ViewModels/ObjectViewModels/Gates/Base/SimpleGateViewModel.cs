using System.Collections.Specialized;
using LogicSimulator.Infrastructure;
using LogicSimulator.Models;
using LogicSimulator.Models.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;

public abstract class SimpleGateViewModel : BaseGateViewModel
{
    private readonly SimpleGateModel _model;

    protected SimpleGateViewModel(SimpleGateModel model) : base(model)
    {
        _model = model;
        OutputPort = new PortViewModel(model.OutputPort, this);
        _inputPorts = new ObservableCollectionEx<PortViewModel, PortModel>(model.InputPorts, portModel => new PortViewModel(portModel, this));
        _inputPorts.CollectionChanged += OnInputPortsCollectionChanged;
    }

    public override IEnumerable<PortViewModel> Ports => [.. InputPorts, OutputPort];

    #region OutputPort

    public PortViewModel OutputPort { get; }

    #endregion

    #region InputPortsCount

    public int InputPortsCount
    {
        get => _inputPorts.Count;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 2);
            var tmp = InputPortsCount;
            if (InputPortsCount != value)
            {
                if (value > tmp)
                {
                    for (var i = 0; i < value - tmp; i++)
                    {
                        _inputPorts.Add(new PortModel());
                    }
                }
                else
                {
                    for (var i = 0; i < tmp - value; i++)
                    {
                        _inputPorts.RemoveAt(_inputPorts.Count - 1);
                    }
                }
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region InputPorts

    private readonly ObservableCollectionEx<PortViewModel, PortModel> _inputPorts;

    public IEnumerable<PortViewModel> InputPorts => _inputPorts;

    #endregion

    #region InputPortsSpacing

    public float InputPortsSpacing
    {
        get => _model.InputPortsSpacing;
        set
        {
            if (Set(_model.InputPortsSpacing, value, _model, (model, value) => model.InputPortsSpacing = value))
            {
                OnSizeChanged();
            }
        }
    }

    #endregion

    protected override void OnSizeChanged()
    {
        CalculateInputPortsPosition();
        OutputPort.Location = new Vector2(Width / 2, 0);
    }

    private void OnInputPortsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_inputPorts.Count < 2)
            throw new InvalidOperationException("Input port count cannot be less than 2.");

        CalculateInputPortsPosition();
    }

    private void CalculateInputPortsPosition()
    {
        var y = -((_inputPorts.Count - _inputPorts.Count % 2) * InputPortsSpacing) / 2;
        for (var i = 0; i < _inputPorts.Count; i++)
        {
            if (_inputPorts.Count % 2 == 0 && _inputPorts.Count / 2 == i)
                y += InputPortsSpacing;
            _inputPorts[i].Rotation = Rotation.Degrees180;
            _inputPorts[i].Location = new Vector2(-Width / 2, y);
            y += InputPortsSpacing;
        }
    }
}
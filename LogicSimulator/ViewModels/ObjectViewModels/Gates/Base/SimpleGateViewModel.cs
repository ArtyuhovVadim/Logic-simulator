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
        _inputPorts = new ObservableCollectionEx<PortViewModel, PortModel>(model.InputPorts, portModel => new PortViewModel(portModel, this));
        _inputPorts.CollectionChanged += OnInputPortsCollectionChanged;
    }

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
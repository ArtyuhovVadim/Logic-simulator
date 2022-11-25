using System.Collections.ObjectModel;
using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents;
using LogicSimulator.Core.LogicComponents.Base;
using LogicSimulator.Core.LogicComponents.Gates;
using LogicSimulator.Core.LogicComponents.Gates.Base;

namespace LogicSimulator.TestWpfApp;

public partial class MainWindow
{
    NorGate _nor1;
    NorGate _nor2;

    Port _inPort1;
    Port _inPort2;

    Port _outPort1;
    Port _outPort2;

    Wire _wire1;
    Wire _wire2;

    private ObservableCollection<LogicComponent> _components = new();

    public MainWindow()
    {
        _nor1 = new(2, 1);
        _nor2 = new(2, 1);

        _inPort1 = _nor1.GetPort(0);
        _inPort2 = _nor2.GetPort(1);

        _outPort1 = _nor1.GetPort(2);
        _outPort2 = _nor2.GetPort(2);

        _wire1 = new(_outPort1, _nor2.GetPort(0));
        _wire2 = new(_outPort2, _nor1.GetPort(1));

        _outPort1.AddWire(_wire1);
        _outPort2.AddWire(_wire2);

        _nor2.GetPort(0).AddWire(_wire1);
        _nor1.GetPort(1).AddWire(_wire2);

        _components.Add(_wire1);
        _components.Add(_wire2);

        AddGatesAndPorts(_nor1);
        AddGatesAndPorts(_nor2);

        InitializeComponent();

        LogicVisualizer.Components = _components;
    }

    private void AddGatesAndPorts(Gate gate)
    {
        _components.Add(gate);

        for (var i = 0; i < gate.PortsCount; i++)
        {
            _components.Add(gate.GetPort(i));
        }
    }
}
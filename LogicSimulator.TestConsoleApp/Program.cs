using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents;
using LogicSimulator.Core.LogicComponents.Gates;

Console.WriteLine("Logic-simulator test app");

var andGate1 = new AndGate(2);
var orGate = new OrGate(2);

andGate1.SetInputPortState(0, PortState.False);

orGate.SetInputPortState(0, PortState.True);
orGate.SetInputPortState(1, PortState.False);

var connection = new Connection(orGate.GetOutputPort(), andGate1.GetInputPort(1));

orGate.Update();
connection.Update();
andGate1.Update();

Console.WriteLine(andGate1.GetOutputPort().State);
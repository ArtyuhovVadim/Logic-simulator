using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents;
using LogicSimulator.Core.LogicComponents.Gates;

Console.WriteLine("Logic-simulator test app");

var nor1 = new NorGate(2, 1);
var nor2 = new NorGate(2, 1);

var inPort1 = nor1.GetPort(0);
var inPort2 = nor2.GetPort(1);

var outPort1 = nor1.GetPort(2);
var outPort2 = nor2.GetPort(2);

var wire1 = new Wire(outPort1, nor2.GetPort(0));
var wire2 = new Wire(outPort2, nor1.GetPort(1));

outPort1.AddWire(wire1);
outPort2.AddWire(wire2);

nor2.GetPort(0).AddWire(wire1);
nor1.GetPort(1).AddWire(wire2);

Simulate(LogicState.False, LogicState.True); // 1 0
Simulate(LogicState.False, LogicState.False); // 0 0
Simulate(LogicState.True, LogicState.False); // 0 0

Console.ReadLine();

void Simulate(LogicState a, LogicState b)
{
    inPort1.State = a;
    inPort2.State = b;

    nor1.GetPort(0).Update();
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("inPort1: " + inPort1.State + " inPort2: " + inPort2.State);
    Console.WriteLine("outPort1: " + outPort1.State + " outPort2: " + outPort2.State);
    Console.ForegroundColor = ConsoleColor.White;
}
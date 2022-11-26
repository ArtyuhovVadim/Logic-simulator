using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents;
using LogicSimulator.Core.LogicComponents.Gates;

Console.WriteLine("Logic-simulator test app");

var nor1 = new NorGate(2, 1);
var nor2 = new NorGate(2, 1);

var outGate1 = new OutputGate();
var outGate2 = new OutputGate();

var inPort1 = nor1.GetPort(0);
var inPort2 = nor2.GetPort(1);

var outPort1 = nor1.GetPort(2);
var outPort2 = nor2.GetPort(2);

var wire1 = new Wire(outPort1, nor2.GetPort(0));
var wire2 = new Wire(outPort2, nor1.GetPort(1));

var outWire1 = new Wire(outGate1.GetPort(0), inPort1);
var outWire2 = new Wire(outGate2.GetPort(0), inPort2);

Simulate(LogicState.False, LogicState.True); // 1 0
Simulate(LogicState.False, LogicState.False); // 0 0
Simulate(LogicState.True, LogicState.False); // 0 0

Console.ReadLine();

void Simulate(LogicState a, LogicState b)
{
    outGate1.State = a; 
    outGate2.State = b;

    outGate1.Update();
    outGate2.Update();
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("outGate1: " + outGate1.State + " outGate2: " + outGate2.State);
    Console.WriteLine("outPort1: " + outPort1.State + " outPort2: " + outPort2.State);
    Console.ForegroundColor = ConsoleColor.White;
}
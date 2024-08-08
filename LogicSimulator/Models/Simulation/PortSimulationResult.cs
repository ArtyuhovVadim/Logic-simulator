using LogicSimulator.Models.Logic;

namespace LogicSimulator.Models.Simulation;

public class PortSimulationResult
{
    public PortSimulationResult(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public List<PortState> States { get; set; } = [];
}
namespace LogicSimulator.Models;

public class PortSimulationResult
{
    public PortSimulationResult(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public List<PortState> States { get; set; } = [];
}
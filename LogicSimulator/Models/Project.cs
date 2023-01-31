using YamlDotNet.Serialization;

namespace LogicSimulator.Models;

public class Project
{
    public const string Extension = ".lsproj";

    public string Version { get; set; }

    public string Name { get; set; }

    [YamlIgnore]
    public IEnumerable<Scheme> Schemes { get; set; }
}
using YamlDotNet.Serialization;

namespace LogicSimulator.Models;

public class Project
{
    public const string Extension = ".lsproj";

    public string Version { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    [YamlIgnore] 
    public IEnumerable<Scheme> Schemes { get; set; } = Enumerable.Empty<Scheme>();
}
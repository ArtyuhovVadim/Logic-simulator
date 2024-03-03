using YamlDotNet.Serialization;

namespace LogicSimulator.Models;

public class Project
{
    public const string Extension = ".lsproj";

    public string Version { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    [YamlIgnore] 
    public List<Scheme> Schemes { get; set; } = [];
}
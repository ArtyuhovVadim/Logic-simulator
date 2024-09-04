using YamlDotNet.Serialization;

namespace LogicSimulator.Models;

public class Project
{
    public const string Extension = ".lsproj";

    [YamlIgnore]
    public string Name { get; set; } = string.Empty;

    public Version Version { get; set; } = Version.Parse("0.0.0.0");

    [YamlIgnore]
    public List<Scheme> Schemes { get; set; } = [];
}
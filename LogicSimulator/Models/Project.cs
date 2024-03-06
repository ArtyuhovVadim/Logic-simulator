using System.IO;
using YamlDotNet.Serialization;

namespace LogicSimulator.Models;

public class Project
{
    public const string Extension = ".lsproj";

    public Version Version { get; set; } = Version.Parse("0.0.0.0");

    [YamlIgnore]
    public FileInfo? FileInfo { get; set; }

    [YamlIgnore]
    public List<Scheme> Schemes { get; set; } = [];
}
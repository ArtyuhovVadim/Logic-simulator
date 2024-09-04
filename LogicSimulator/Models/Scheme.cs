using LogicSimulator.Models.Objects.Base;
using YamlDotNet.Serialization;

namespace LogicSimulator.Models;

public class Scheme
{
    public const string Extension = ".lss";

    [YamlIgnore]
    public string Name { get; set; } = string.Empty;

    public Version Version { get; set; } = Version.Parse("0.0.0.0");

    public List<BaseObjectModel> Objects { get; set; } = [];
}
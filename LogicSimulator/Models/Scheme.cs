using System.IO;
using LogicSimulator.Models.Base;
using YamlDotNet.Serialization;

namespace LogicSimulator.Models;

public class Scheme
{
    public const string Extension = ".lss";

    public Version Version { get; set; } = Version.Parse("0.0.0.0");

    public List<BaseObjectModel> Objects { get; set; } = [];

    [YamlIgnore]
    public FileInfo? FileInfo { get; set; }
}
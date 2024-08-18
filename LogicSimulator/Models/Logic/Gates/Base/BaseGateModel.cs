using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Objects.Base;
using System.Windows.Media;
using LogicSimulator.Shared.Models;
using YamlDotNet.Serialization;

namespace LogicSimulator.Models.Logic.Gates.Base;

public abstract class BaseGateModel : BaseObjectModel
{
    public string Name { get; set; } = string.Empty;

    public Color FillColor { get; set; } = Colors.White;

    public Color StrokeColor { get; set; } = Colors.Black;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Smallest;

    public ulong Delay { get; set; }

    public float Scale { get; set; } = 1f;

    [YamlIgnore]
    public IEnumerable<PortModel> Ports => InputPorts.Concat(OutputPorts);

    [YamlIgnore]
    public virtual IEnumerable<PortModel> InputPorts => [];

    [YamlIgnore]
    public virtual IEnumerable<PortModel> OutputPorts => [];

    public abstract void Accept(IGateModelVisitor visitor);
}
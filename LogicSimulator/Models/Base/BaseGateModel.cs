using LogicSimulator.Core.Gates.Base;
using LogicSimulator.Scene;
using System.Windows.Media;
using YamlDotNet.Serialization;

namespace LogicSimulator.Models.Base;

public abstract class BaseGateModel : BaseObjectModel
{
    [YamlIgnore]
    public abstract BaseGate LogicModel { get; }

    public Color FillColor { get; set; } = Colors.White;

    public Color StrokeColor { get; set; } = Colors.Black;

    public float StrokeThickness { get; set; } = 10f;

    public StrokeThicknessType StrokeThicknessType { get; set; } = StrokeThicknessType.Smallest;
}
using LogicSimulator.Models.Objects.Base;
using LogicSimulator.Scene.Models;
using System.Windows.Media;

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
}
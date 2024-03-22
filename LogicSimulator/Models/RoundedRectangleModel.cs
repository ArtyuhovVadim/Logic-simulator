namespace LogicSimulator.Models;

public class RoundedRectangleModel : RectangleModel
{
    public float RadiusX { get; set; }

    public float RadiusY { get; set; }

    public override RoundedRectangleModel MakeClone() => (RoundedRectangleModel)MemberwiseClone();
}
namespace LogicSimulator.Utils;

public static class DoubleExtensionMethods
{
    public static double Map(this double x, double inMin, double inMax, double outMin, double outMax) =>
        (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    public static double Clamp(this double x, double min, double max) =>
        x < min ? min : x > max ? max : x;
}
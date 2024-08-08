namespace LogicSimulator.Shared.Models;

public interface ICloneable<out T> : ICloneable where T : class
{
    T MakeClone();

    object ICloneable.Clone() => MakeClone();
}
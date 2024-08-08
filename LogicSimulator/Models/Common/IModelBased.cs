namespace LogicSimulator.Models.Common;

public interface IModelBased<out T>
{
    T Model { get; }
}
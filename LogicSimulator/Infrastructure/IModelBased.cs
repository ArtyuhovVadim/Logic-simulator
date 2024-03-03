namespace LogicSimulator.Infrastructure;

public interface IModelBased<out T>
{
    T Model { get; }
}
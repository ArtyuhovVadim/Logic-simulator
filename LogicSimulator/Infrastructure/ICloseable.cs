namespace LogicSimulator.Infrastructure;

public interface ICloseable
{
    event Action? Closed;
}
namespace LogicSimulator.Models.Common;

public interface ICloseable
{
    event Action? Closed;
}
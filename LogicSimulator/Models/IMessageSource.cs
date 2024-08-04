namespace LogicSimulator.Models;

public interface IMessageSource
{
    string Name { get; }

    void GoTo();
}
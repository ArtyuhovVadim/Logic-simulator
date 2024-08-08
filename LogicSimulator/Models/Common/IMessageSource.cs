namespace LogicSimulator.Models.Common;

public interface IMessageSource
{
    string Name { get; }

    void GoTo();
}
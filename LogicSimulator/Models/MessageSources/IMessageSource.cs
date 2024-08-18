namespace LogicSimulator.Models.MessageSources;

public interface IMessageSource
{
    string Name { get; }

    void GoTo();
}
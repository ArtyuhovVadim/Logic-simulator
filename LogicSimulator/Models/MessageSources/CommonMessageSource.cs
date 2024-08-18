namespace LogicSimulator.Models.MessageSources;

public class CommonMessageSource : IMessageSource
{
    private readonly Action? _goToFunc;

    public CommonMessageSource(string name) => Name = name;

    public CommonMessageSource(string name, Action goToFunc) : this(name) => _goToFunc = goToFunc;

    public string Name { get; }

    public void GoTo() => _goToFunc?.Invoke();
}
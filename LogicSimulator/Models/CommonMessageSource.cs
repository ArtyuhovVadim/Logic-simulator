namespace LogicSimulator.Models;

public class CommonMessageSource : IMessageSource
{
    private readonly Action? _goToFunc;

    public CommonMessageSource(string name) => Name = name;

    public CommonMessageSource(Action goToFunc, string name) : this(name) => _goToFunc = goToFunc;

    public string Name { get; }

    public void GoTo() => _goToFunc?.Invoke();
}
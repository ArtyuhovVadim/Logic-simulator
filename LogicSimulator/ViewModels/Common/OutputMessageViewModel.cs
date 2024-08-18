using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Models.Common;
using LogicSimulator.Models.MessageSources;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.Common;

public class OutputMessageViewModel : BindableBase
{
    public OutputMessageViewModel() { }

    public OutputMessageViewModel(string text, ValidationRuleLevel level, IMessageSource? source) : this(text, level.ToMessageType(), DateTime.Now, source) { }

    public OutputMessageViewModel(string text, MessageType type, IMessageSource? source) : this(text, type, DateTime.Now, source) { }

    public OutputMessageViewModel(string text, MessageType type, DateTime time, IMessageSource? source)
    {
        _text = text;
        _type = type;
        _time = time;
        _source = source;
    }

    #region Type

    private MessageType _type = MessageType.Information;

    public MessageType Type
    {
        get => _type;
        set => Set(ref _type, value);
    }

    #endregion

    #region Time

    private DateTime _time;

    public DateTime Time
    {
        get => _time;
        set => Set(ref _time, value);
    }

    #endregion

    #region Text

    private string _text = string.Empty;

    public string Text
    {
        get => _text;
        set => Set(ref _text, value);
    }

    #endregion

    #region Source

    private IMessageSource? _source;

    public IMessageSource? Source
    {
        get => _source;
        set => Set(ref _source, value);
    }

    #endregion
}
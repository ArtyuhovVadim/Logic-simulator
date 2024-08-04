using LogicSimulator.Infrastructure;
using LogicSimulator.Models;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class OutputMessageViewModel : BindableBase
{
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
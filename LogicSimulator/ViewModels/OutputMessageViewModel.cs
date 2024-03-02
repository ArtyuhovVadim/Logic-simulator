using LogicSimulator.Infrastructure;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class OutputMessageViewModel : BindableBase
{
    #region Text

    private string _text = string.Empty;

    public string Text
    {
        get => _text;
        set => Set(ref _text, value);
    }

    #endregion

    #region Type

    private MessageType _type = MessageType.Information;

    public MessageType Type
    {
        get => _type;
        set => Set(ref _type, value);
    }

    #endregion
}
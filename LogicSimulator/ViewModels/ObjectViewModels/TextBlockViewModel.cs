using System.Windows.Media;
using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class TextBlockViewModel : BaseObjectViewModel
{
    #region Text

    private string _text = "Text";

    [Editable]
    public string Text
    {
        get => _text;
        set => Set(ref _text, value);
    }

    #endregion

    #region FontName

    private string _fontName = "Times New Roman";

    [Editable]
    public string FontName
    {
        get => _fontName;
        set => Set(ref _fontName, value);
    }

    #endregion

    #region FontSize

    private float _fontSize = 24f;

    [Editable]
    public float FontSize
    {
        get => _fontSize;
        set => Set(ref _fontSize, value);
    }

    #endregion

    #region IsBold

    private bool _isBold;

    [Editable]
    public bool IsBold
    {
        get => _isBold;
        set => Set(ref _isBold, value);
    }

    #endregion

    #region IsItalic

    private bool _isItalic;

    [Editable]
    public bool IsItalic
    {
        get => _isItalic;
        set => Set(ref _isItalic, value);
    }

    #endregion

    #region IsUnderlined

    private bool _isUnderlined;

    [Editable]
    public bool IsUnderlined
    {
        get => _isUnderlined;
        set => Set(ref _isUnderlined, value);
    }

    #endregion

    #region IsCross

    private bool _isCross;

    [Editable]
    public bool IsCross
    {
        get => _isCross;
        set => Set(ref _isCross, value);
    }

    #endregion

    #region TextColor

    private Color _textColor = Colors.Black;

    [Editable]
    public Color TextColor
    {
        get => _textColor;
        set => Set(ref _textColor, value);
    }

    #endregion
}
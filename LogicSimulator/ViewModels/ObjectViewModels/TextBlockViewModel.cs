using System.Windows.Media;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class TextBlockViewModel : BaseObjectViewModel
{
    public TextBlockViewModel(TextBlockModel model) => Model = model;

    public override TextBlockModel Model { get; }

    #region Text

    public string Text
    {
        get => Model.Text;
        set => Set(Model.Text, value, Model, (model, value) => model.Text = value);
    }

    #endregion

    #region FontName

    public string FontName
    {
        get => Model.FontName;
        set => Set(Model.FontName, value, Model, (model, value) => model.FontName= value);
    }

    #endregion

    #region FontSize

    public float FontSize
    {
        get => Model.FontSize;
        set => Set(Model.FontSize, value, Model, (model, value) => model.FontSize = value);
    }

    #endregion

    #region IsBold

    public bool IsBold
    {
        get => Model.IsBold;
        set => Set(Model.IsBold, value, Model, (model, value) => model.IsBold = value);
    }

    #endregion

    #region IsItalic

    public bool IsItalic
    {
        get => Model.IsItalic;
        set => Set(Model.IsItalic, value, Model, (model, value) => model.IsItalic = value);
    }

    #endregion

    #region IsUnderlined

    public bool IsUnderlined
    {
        get => Model.IsUnderlined;
        set => Set(Model.IsUnderlined, value, Model, (model, value) => model.IsUnderlined = value);
    }

    #endregion

    #region IsCross

    public bool IsCross
    {
        get => Model.IsCross;
        set => Set(Model.IsCross, value, Model, (model, value) => model.IsCross = value);
    }

    #endregion

    #region TextColor

    public Color TextColor
    {
        get => Model.TextColor;
        set => Set(Model.TextColor, value, Model, (model, value) => model.TextColor = value);
    }

    #endregion

    public override TextBlockViewModel MakeClone() => new(Model.MakeClone());
}
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;
using System.Windows.Media;
using LogicSimulator.Models;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates;

public class AndGateViewModel : BaseGateViewModel
{
    public AndGateViewModel(AndGateModel model) => Model = model;

    public override AndGateModel Model { get; }

    #region FillColor

    public Color FillColor
    {
        get => Model.FillColor;
        set => Set(Model.FillColor, value, Model, (model, value) => model.FillColor = value);
    }

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => Model.StrokeColor;
        set => Set(Model.StrokeColor, value, Model, (model, value) => model.StrokeColor = value);
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => Model.StrokeThickness;
        set => Set(Model.StrokeThickness, value, Model, (model, value) => model.StrokeThickness = value);
    }

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => Model.StrokeThicknessType;
        set => Set(Model.StrokeThicknessType, value, Model, (model, value) => model.StrokeThicknessType = value);
    }

    #endregion

    public override AndGateViewModel MakeClone() => new(Model.MakeClone());
}
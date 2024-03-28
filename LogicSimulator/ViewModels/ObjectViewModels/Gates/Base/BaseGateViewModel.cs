using System.Windows.Media;
using LogicSimulator.Models.Base;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;

public abstract class BaseGateViewModel : BaseObjectViewModel
{
    public override BaseGateModel Model { get; }

    protected BaseGateViewModel(BaseGateModel model) => Model = model;

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
}
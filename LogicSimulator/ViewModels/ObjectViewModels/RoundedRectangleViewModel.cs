using System.Windows.Media;
using LogicSimulator.Models;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class RoundedRectangleViewModel : BaseObjectViewModel
{
    public RoundedRectangleViewModel(RoundedRectangleModel model) => Model = model;

    public override RoundedRectangleModel Model { get; }

    #region Width

    public float Width
    {
        get => Model.Width;
        set => Set(Model.Width, value, Model, (model, value) => model.Width = value);
    }

    #endregion

    #region Height

    public float Height
    {
        get => Model.Height;
        set => Set(Model.Height, value, Model, (model, value) => model.Height = value);
    }

    #endregion

    #region RadiusX

    public float RadiusX
    {
        get => Model.RadiusX;
        set => Set(Model.RadiusX, value, Model, (model, value) => model.RadiusX = value);
    }

    #endregion

    #region RadiusY

    public float RadiusY
    {
        get => Model.RadiusY;
        set => Set(Model.RadiusY, value, Model, (model, value) => model.RadiusY = value);
    }

    #endregion

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

    #region IsFilled

    public bool IsFilled
    {
        get => Model.IsFilled;
        set => Set(Model.IsFilled, value, Model, (model, value) => model.IsFilled = value);
    }

    #endregion

    public override RoundedRectangleViewModel MakeClone() => new(Model.MakeClone());
}
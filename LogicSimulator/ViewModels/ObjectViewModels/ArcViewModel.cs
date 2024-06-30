using LogicSimulator.Models;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class ArcViewModel : BaseObjectViewModel
{
    public ArcViewModel(ArcModel model) => Model = model;

    public override ArcModel Model { get; }

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

    #region StartAngle

    public float StartAngle
    {
        get => Model.StartAngle;
        set => Set(Model.StartAngle, value, Model, (model, value) => model.StartAngle = value);
    }

    #endregion

    #region EndAngle

    public float EndAngle
    {
        get => Model.EndAngle;
        set => Set(Model.EndAngle, value, Model, (model, value) => model.EndAngle = value);
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

    public override ArcViewModel MakeClone() => new(Model.MakeClone());
}
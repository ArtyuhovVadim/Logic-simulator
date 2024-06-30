using LogicSimulator.Models;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class BezierCurveViewModel : BaseObjectViewModel
{
    public BezierCurveViewModel(BezierCurveModel model) => Model = model;

    public override BezierCurveModel Model { get; }

    #region Point1

    public Vector2 Point1
    {
        get => Model.Point1;
        set => Set(Model.Point1, value, Model, (model, value) => model.Point1 = value);
    }

    #endregion

    #region Point2

    public Vector2 Point2
    {
        get => Model.Point2;
        set => Set(Model.Point2, value, Model, (model, value) => model.Point2 = value);
    }

    #endregion

    #region Point3

    public Vector2 Point3
    {
        get => Model.Point3;
        set => Set(Model.Point3, value, Model, (model, value) => model.Point3 = value);
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

    public override BezierCurveViewModel MakeClone() => new(Model.MakeClone());
}
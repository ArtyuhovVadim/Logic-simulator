using LogicSimulator.Models;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using System.Windows.Media;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class PathViewModel : BaseObjectViewModel
{
    public PathViewModel(PathModel model) => Model = model;

    public override PathModel Model { get; }

    #region Geometry

    public string Geometry
    {
        get => Model.Geometry;
        set => Set(Model.Geometry, value, Model, (model, value) => model.Geometry = value);
    }

    #endregion

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

    #region Scale

    public float Scale
    {
        get => Model.Scale;
        set => Set(Model.Scale, value, Model, (model, value) => model.Scale = value);
    }

    #endregion

    #region OriginPosition

    public OriginPosition OriginPosition
    {
        get => Model.OriginPosition;
        set => Set(Model.OriginPosition, value, Model, (model, value) => model.OriginPosition = value);
    }

    #endregion

    #region Stretch

    public Stretch Stretch
    {
        get => Model.Stretch;
        set => Set(Model.Stretch, value, Model, (model, value) => model.Stretch = value);
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

    #region SelectionPadding

    public float SelectionPadding
    {
        get => Model.SelectionPadding;
        set => Set(Model.SelectionPadding, value, Model, (model, value) => model.SelectionPadding = value);
    }

    #endregion

    #region FillColor

    public Color FillColor
    {
        get => Model.FillColor;
        set => Set(Model.FillColor, value, Model, (model, value) => model.FillColor = value);
    }

    #endregion

    #region IsAntiAliased

    public bool IsAntiAliased
    {
        get => Model.IsAntiAliased;
        set => Set(Model.IsAntiAliased, value, Model, (model, value) => model.IsAntiAliased = value);
    }

    #endregion

    #region IsStroked

    public bool IsStroked
    {
        get => Model.IsStroked;
        set => Set(Model.IsStroked, value, Model, (model, value) => model.IsStroked = value);
    }

    #endregion

    #region IsFilled

    public bool IsFilled
    {
        get => Model.IsFilled;
        set => Set(Model.IsFilled, value, Model, (model, value) => model.IsFilled = value);
    }

    #endregion

    public override PathViewModel MakeClone() => new(Model);
}
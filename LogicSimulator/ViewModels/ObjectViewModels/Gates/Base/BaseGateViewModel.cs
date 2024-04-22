using System.Windows.Media;
using LogicSimulator.Models.Base;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;

public abstract class BaseGateViewModel : BaseObjectViewModel
{
    public override BaseGateModel Model { get; }

    protected BaseGateViewModel(BaseGateModel model) => Model = model;

    #region Scale

    public float Scale
    {
        get => Model.Scale;
        set
        {
            if (Set(Model.Scale, value, Model, (model, value) => model.Scale = value))
            {
                OnPropertyChanged(nameof(Width));
                OnPropertyChanged(nameof(Height));
                OnSizeChanged();
            }
        }
    }

    #endregion

    #region Width

    private float _width;

    public float Width
    {
        get => _width * Scale;
        set
        {
            if (Set(ref _width, value))
            {
                OnSizeChanged();
            }
        }
    }

    #endregion

    #region Height

    private float _height;

    public float Height
    {
        get => _height * Scale;
        set
        {
            if (Set(ref _height, value))
                OnSizeChanged();
        }
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

    #region Delay

    public ulong Delay
    {
        get => Model.Delay;
        set => Set(Model.Delay, value, Model, (model, value) => model.Delay = value);
    }

    #endregion

    protected abstract void OnSizeChanged();
}
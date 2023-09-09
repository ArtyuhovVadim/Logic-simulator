namespace LogicSimulator.ViewModels.ObjectViewModels;

public class RoundedRectangleViewModel : RectangleViewModel
{
    #region RadiusX

    private float _radiusX;

    public float RadiusX
    {
        get => _radiusX;
        set => Set(ref _radiusX, value);
    }

    #endregion

    #region RadiusY

    private float _radiusY;

    public float RadiusY
    {
        get => _radiusY;
        set => Set(ref _radiusY, value);
    }

    #endregion
}
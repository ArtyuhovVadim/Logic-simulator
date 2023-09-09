using System.Windows.Media;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class RectangleViewModel : SceneObjectViewModel
{
    #region Width

    private float _width;

    public float Width
    {
        get => _width;
        set => Set(ref _width, value);
    }

    #endregion

    #region Height

    private float _height;

    public float Height
    {
        get => _height;
        set => Set(ref _height, value);
    }

    #endregion

    #region Background

    private Color _background = Colors.Blue;

    public Color Background
    {
        get => _background;
        set => Set(ref _background, value);
    }

    #endregion
}
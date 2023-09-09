using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.ObjectViewModels.Base;

public abstract class BaseObjectViewModel : BindableBase
{
    #region Location

    private Vector2 _location;

    public Vector2 Location
    {
        get => _location;
        set => Set(ref _location, value);
    }

    #endregion

    #region Rotation

    private Rotation _rotation = Rotation.Degrees0;

    public Rotation Rotation
    {
        get => _rotation;
        set => Set(ref _rotation, value);
    }

    #endregion
}
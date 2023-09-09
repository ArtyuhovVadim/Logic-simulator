using LogicSimulator.ViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.ObjectViewModels.Base;

public class SceneObjectViewModel : BindableBase
{
    #region Location

    private Vector2 _location;

    public Vector2 Location
    {
        get => _location;
        set => Set(ref _location, value);
    }

    #endregion
}
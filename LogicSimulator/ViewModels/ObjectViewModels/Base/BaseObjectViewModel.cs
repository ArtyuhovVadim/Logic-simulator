using LogicSimulator.Infrastructure;
using SharpDX;
using WpfExtensions.Mvvm;
using YamlDotNet.Serialization;

namespace LogicSimulator.ViewModels.ObjectViewModels.Base;

//TODO: Сделать модели для объектов сцены.
public abstract class BaseObjectViewModel : BindableBase, ICloneable<BaseObjectViewModel>, IModelBased<BaseObjectViewModel>
{
    [YamlIgnore]
    public BaseObjectViewModel Model => this;

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

    #region IsSelected

    private bool _isSelected;

    [YamlIgnore]
    public bool IsSelected
    {
        get => _isSelected;
        set => Set(ref _isSelected, value);
    }

    #endregion

    public void RotateClockwise() => Rotation = Rotation switch
    {
        Rotation.Degrees0 => Rotation.Degrees90,
        Rotation.Degrees90 => Rotation.Degrees180,
        Rotation.Degrees180 => Rotation.Degrees270,
        Rotation.Degrees270 => Rotation.Degrees0,
        _ => throw new ArgumentOutOfRangeException()
    };

    public void RotateCounterclockwise() => Rotation = Rotation switch
    {
        Rotation.Degrees0 => Rotation.Degrees270,
        Rotation.Degrees90 => Rotation.Degrees0,
        Rotation.Degrees180 => Rotation.Degrees90,
        Rotation.Degrees270 => Rotation.Degrees180,
        _ => throw new ArgumentOutOfRangeException()
    };

    public abstract BaseObjectViewModel MakeClone();
}
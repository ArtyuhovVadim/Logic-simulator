using LogicSimulator.Infrastructure;
using SharpDX;
using LogicSimulator.Models.Base;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.ObjectViewModels.Base;

public abstract class BaseObjectViewModel : BindableBase, ICloneable<BaseObjectViewModel>, IModelBased<BaseObjectModel>
{
    public abstract BaseObjectModel Model { get; }

    #region Location

    public Vector2 Location
    {
        get => Model.Location;
        set => Set(Model.Location, value, Model, (model, value) => model.Location = value);
    }

    #endregion

    #region Rotation

    public Rotation Rotation
    {
        get => Model.Rotation;
        set => Set(Model.Rotation, value, Model, (model, value) => model.Rotation = value);
    }

    #endregion

    #region IsSelected

    private bool _isSelected;

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
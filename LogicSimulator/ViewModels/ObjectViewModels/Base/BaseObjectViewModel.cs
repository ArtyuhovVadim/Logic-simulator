using LogicSimulator.Infrastructure;
using SharpDX;
using System.Runtime.CompilerServices;
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

    //TODO: Переместить в WpfExtensions
    protected bool Set<TModel, T>(T oldValue, T newValue, TModel model, Action<TModel, T> callback, [CallerMemberName] string? propertyName = null) where TModel : class
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(callback);

        if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            return false;

        callback(model, newValue);
        OnPropertyChanged(propertyName);

        return true;
    }

    //TODO: Переместить в WpfExtensions
    protected bool Set<TModel, T>(T oldValue, T newValue, IEqualityComparer<T> comparer, TModel model, Action<TModel, T> callback, [CallerMemberName] string? propertyName = null) where TModel : class
    {
        ArgumentNullException.ThrowIfNull(comparer);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(callback);

        if (comparer.Equals(oldValue, newValue))
            return false;

        callback(model, newValue);
        OnPropertyChanged(propertyName);

        return true;
    }
}
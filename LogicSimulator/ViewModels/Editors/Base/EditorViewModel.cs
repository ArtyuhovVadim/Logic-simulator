using System.ComponentModel;
using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.EditorLayout;
using LogicSimulator.ViewModels.Editors.Base.Properties;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.Editors.Base;

public abstract class EditorViewModel : BindableBase
{
    private EditorLayout? _layout;

    private IReadOnlyCollection<INotifyPropertyChanged>? _objectsToEdit;

    public EditorLayout Layout => _layout ??= CreateLayout();

    public IReadOnlyCollection<object> Objects => _objectsToEdit!;

    public void SetObjectsToEdit<T>(ICollection<T> objects) where T : class, INotifyPropertyChanged
    {
        if (_objectsToEdit is not null)
        {
            Layout.EndEdit();

            foreach (var obj in _objectsToEdit)
            {
                obj.PropertyChanged -= OnPropertyChanged;
            }

            _objectsToEdit = null;
        }

        if (objects.Count == 0) return;

        _objectsToEdit = new List<INotifyPropertyChanged>(objects);

        foreach (var obj in _objectsToEdit)
        {
            obj.PropertyChanged += OnPropertyChanged;
        }

        Layout.StartEdit();
        Layout.RaisePropertyChangeForAllProperties();

        OnPropertyChanged(nameof(Layout));
    }

    public void StopObjectsEdit()
    {
        if (_objectsToEdit is null) return;

        Layout.EndEdit();

        foreach (var obj in _objectsToEdit)
        {
            obj.PropertyChanged -= OnPropertyChanged;
        }

        _objectsToEdit = null;
    }

    protected void OnPropertyChanged(object? sender, PropertyChangedEventArgs e) => Layout.PropertyChange(e.PropertyName!);

    protected abstract EditorLayout CreateLayout();

    public static void ConfigureAsPositionVector(Vector2PropertyViewModel prop)
    {
        prop.NumberSuffix = Constants.MillimetreSuffix;
        prop.DisplayCoefficient = Constants.MillimetreToPixelFactor;
    }

    public static void ConfigureAsSizeNumber(NumberPropertyViewModel<float> prop)
    {
        prop.MinNumber = 1;
        prop.NumberSuffix = Constants.MillimetreSuffix;
        prop.DisplayCoefficient = (float)Constants.MillimetreToPixelFactor;
    }

    public static void ConfigureAsFontSizeNumber(NumberPropertyViewModel<float> prop)
    {
        prop.MinNumber = 6;
        prop.NumberSuffix = Constants.PixelSuffix;
    }

    public static void ConfigureAsAngleNumber(NumberPropertyViewModel<float> prop)
    {
        prop.NumberSuffix = Constants.AngleSuffix;
    }
}
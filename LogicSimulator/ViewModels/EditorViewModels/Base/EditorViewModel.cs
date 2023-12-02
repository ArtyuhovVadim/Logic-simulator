using System.ComponentModel;
using LogicSimulator.ViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class EditorViewModel : BindableBase
{
    private EditorLayout _layout;

    private IEnumerable<INotifyPropertyChanged> _objectsToEdit;

    public EditorLayout Layout => _layout ??= CreateLayout();

    public IEnumerable<object> Objects => _objectsToEdit;

    public void SetObjectsToEdit<T>(IEnumerable<T> objects) where T : class, INotifyPropertyChanged
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

        if (!objects.Any()) return;

        _objectsToEdit = new List<INotifyPropertyChanged>(objects);

        foreach (var obj in _objectsToEdit)
        {
            obj.PropertyChanged += OnPropertyChanged;
        }

        Layout.StartEdit();
        Layout.RaisePropertyChangeForAllProperties();

        OnPropertyChanged(nameof(Layout));
    }

    protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => Layout.PropertyChange(e.PropertyName);

    protected abstract EditorLayout CreateLayout();
}
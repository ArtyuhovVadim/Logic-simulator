﻿using System.ComponentModel;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public abstract class EditorViewModel : BindableBase
{
    private EditorLayout? _layout;

    private IEnumerable<INotifyPropertyChanged>? _objectsToEdit;

    public EditorLayout Layout => _layout ??= CreateLayout();

    public IEnumerable<object> Objects => _objectsToEdit!;

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
}
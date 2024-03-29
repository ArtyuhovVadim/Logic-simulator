﻿namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class FontNamePropertyViewModel : SinglePropertyViewModel
{
    #region IsValueUndefined

    private bool _isValueUndefined;

    public bool IsValueUndefined
    {
        get => _isValueUndefined;
        set => Set(ref _isValueUndefined, value);
    }

    #endregion

    protected override string GetPropertyValue(IEnumerable<object> objects)
    {
        var firstObj = objects.First();

        IsValueUndefined = objects.Any(o => !Equals(GetValue<string>(o), GetValue<string>(firstObj)));

        return GetValue<string>(firstObj);
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        IsValueUndefined = false;

        var newValue = (string)value;

        foreach (var obj in objects)
        {
            SetValue(obj, newValue);
        }
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new FontNamePropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };
}
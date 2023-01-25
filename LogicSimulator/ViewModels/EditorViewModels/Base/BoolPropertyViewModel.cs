﻿using System.Collections.Generic;
using System.Linq;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public class BoolPropertyViewModel : SinglePropertyViewModel
{
    #region IsValueUndefined

    private bool _isValueUndefined;

    public bool IsValueUndefined
    {
        get => _isValueUndefined;
        set => Set(ref _isValueUndefined, value);
    }

    #endregion  

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        IsValueUndefined = objects.Any(o => !Equals(PropertyInfo.GetValue(o), PropertyInfo.GetValue(objects.First())));
        return PropertyInfo.GetValue(objects.First());
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value)
    {
        if (IsValueUndefined) return;

        foreach (var obj in objects)
        {
            PropertyInfo.SetValue(obj, value);
        }
    }
}
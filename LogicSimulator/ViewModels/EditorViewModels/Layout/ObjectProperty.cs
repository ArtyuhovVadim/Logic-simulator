using System;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class ObjectProperty : BindableBase
{
    private Type _objectPropertyType;
    private string _objectPropertyName;

    public string ObjectPropertyName
    {
        get => _objectPropertyName;
        set => Set(ref _objectPropertyName, value);
    }

    public Type ObjectPropertyType
    {
        get => _objectPropertyType;
        set => Set(ref _objectPropertyType, value);
    }

    public ObjectProperty(string objectPropertyObjectPropertyName, Type objectPropertyType)
    {
        ObjectPropertyName = objectPropertyObjectPropertyName;
        ObjectPropertyType = objectPropertyType;
    }
}
using System;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class ObjectProperty
{
    public string ObjectPropertyName { get; }

    public Type ObjectPropertyType { get; }

    public ObjectProperty(string objectPropertyObjectPropertyName, Type objectPropertyType)
    {
        ObjectPropertyName = objectPropertyObjectPropertyName;
        ObjectPropertyType = objectPropertyType;
    }
}
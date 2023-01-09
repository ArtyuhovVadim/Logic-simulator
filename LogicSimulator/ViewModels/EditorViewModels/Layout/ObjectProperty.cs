using System;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class ObjectProperty : BindableBase
{
    public string ObjectPropertyName { get; set; }

    public Type ObjectPropertyType { get; set; }

    public ObjectProperty(string objectPropertyObjectPropertyName, Type objectPropertyType)
    {
        ObjectPropertyName = objectPropertyObjectPropertyName;
        ObjectPropertyType = objectPropertyType;
    }
}
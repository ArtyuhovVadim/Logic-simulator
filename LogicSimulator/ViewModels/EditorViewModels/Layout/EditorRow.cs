using System.Collections.Generic;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorRow
{
    public string Name { get; } = string.Empty;

    public IEnumerable<ObjectProperty> ObjectProperties { get; }

    public EditorRow(ObjectProperty objectProperty) => ObjectProperties = new[] { objectProperty };
    public EditorRow(string name, ObjectProperty objectProperty) : this(objectProperty) => Name = name;

    public EditorRow(IEnumerable<ObjectProperty> objectProperties) => ObjectProperties = objectProperties;
    public EditorRow(string name, IEnumerable<ObjectProperty> objectProperties) : this(objectProperties) => Name = name;
}
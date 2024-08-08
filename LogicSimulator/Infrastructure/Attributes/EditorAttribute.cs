namespace LogicSimulator.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class EditorAttribute : Attribute
{
    public Type ObjectType { get; set; }

    public EditorAttribute(Type objectType) => ObjectType = objectType;
}
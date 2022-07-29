using YamlDotNet.Serialization;

namespace LogicSimulator.Scene.SceneObjects.Base;

public abstract class EditableSceneObject : BaseSceneObject
{
    [YamlIgnore]
    public abstract Node[] Nodes { get; }
}
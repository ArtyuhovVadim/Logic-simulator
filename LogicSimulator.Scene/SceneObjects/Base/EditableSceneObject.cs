using LogicSimulator.Scene.Nodes;
using YamlDotNet.Serialization;

namespace LogicSimulator.Scene.SceneObjects.Base;

public abstract class EditableSceneObject : BaseSceneObject
{
    [YamlIgnore]
    public abstract AbstractNode[] Nodes { get; }
}
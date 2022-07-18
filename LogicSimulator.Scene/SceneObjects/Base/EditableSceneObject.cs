namespace LogicSimulator.Scene.SceneObjects.Base;

public abstract class EditableSceneObject : BaseSceneObject
{
    public abstract Node[] Nodes { get; }
}
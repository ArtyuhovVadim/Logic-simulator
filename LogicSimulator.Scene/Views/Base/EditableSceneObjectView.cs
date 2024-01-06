using LogicSimulator.Scene.Nodes;

namespace LogicSimulator.Scene.Views.Base;

public abstract class EditableSceneObjectView : SceneObjectView
{
    public abstract IEnumerable<AbstractNode> Nodes { get; }
}
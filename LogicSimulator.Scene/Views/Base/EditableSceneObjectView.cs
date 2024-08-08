using LogicSimulator.Scene.Nodes;
using LogicSimulator.Shared.Models;

namespace LogicSimulator.Scene.Views.Base;

public abstract class EditableSceneObjectView : SceneObjectView, IEditable
{
    public abstract IEnumerable<AbstractNode> Nodes { get; }

    IEnumerable<IEditableObjectNode> IEditable.Nodes => Nodes;
}
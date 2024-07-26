namespace LogicSimulator.Shared;

public interface IEditable : IHitTestable
{
    bool IsSelected { get; }

    IEnumerable<IEditableObjectNode> Nodes { get; }
}
using LogicSimulator.Shared.Models.HitTest;

namespace LogicSimulator.Shared.Models;

public interface IEditable : IHitTestable
{
    bool IsSelected { get; }

    IEnumerable<IEditableObjectNode> Nodes { get; }
}
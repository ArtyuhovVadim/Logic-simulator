using LogicSimulator.Shared.Models.HitTest;

namespace LogicSimulator.Shared.Models;

public interface ISelectable : IHitTestable
{
    bool IsSelected { get; }

    void Select();

    void Unselect();
}
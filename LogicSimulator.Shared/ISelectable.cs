namespace LogicSimulator.Shared;

public interface ISelectable : IHitTestable
{
    bool IsSelected { get; }

    void Select();

    void Unselect();
}
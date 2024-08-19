using SharpDX;

namespace LogicSimulator.Models.Input;

public record DragInputArgs(Vector2 StartPosition, Vector2 Position, Vector2 Delta) : InputArgs(Position);
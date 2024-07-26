using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public record DragInputArgs(Vector2 StartPosition, Vector2 Position, Vector2 Delta) : InputArgs(Position);
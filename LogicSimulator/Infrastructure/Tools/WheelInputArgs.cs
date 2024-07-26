using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public record WheelInputArgs(Vector2 Position, float Delta) : InputArgs(Position);
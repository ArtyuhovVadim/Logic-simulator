using SharpDX;

namespace LogicSimulator.Models.Input;

public record WheelInputArgs(Vector2 Position, float Delta) : InputArgs(Position);
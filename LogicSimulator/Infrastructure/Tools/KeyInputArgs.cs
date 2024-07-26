using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public record KeyInputArgs(Key Key, ModifierKeys KeyModifiers, Vector2 Position) : InputArgs(Position);
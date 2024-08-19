using SharpDX;

namespace LogicSimulator.Models.Input;

public record KeyInputArgs(Key Key, ModifierKeys KeyModifiers, Vector2 Position) : InputArgs(Position);
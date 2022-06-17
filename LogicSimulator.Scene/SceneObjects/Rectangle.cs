using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;

namespace LogicSimulator.Scene.SceneObjects;

public class Rectangle : BaseSceneObject
{
    public Vector2 Location { get; set; } = Vector2.Zero;

    public float Width { get; set; }

    public float Height { get; set; }

    public float StrokeThickness { get; set; } = 1f;

    public Color4 FillColor { get; set; } = Color4.White;

    public Color4 StrokeColor { get; set; } = Color4.Black;

    public override void Render(ObjectRenderer renderer) => renderer.Render(this);
}
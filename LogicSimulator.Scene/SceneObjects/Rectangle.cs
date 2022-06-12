using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;

namespace LogicSimulator.Scene.SceneObjects;

public class Rectangle : BaseSceneObject
{
    public Vector2 Location { get; set; } = Vector2.Zero;

    public Vector2 Size { get; set; } = Vector2.Zero;

    public float StrokeWidth { get; set; } = 1f;

    public Color4 StrokeColor { get; set; } = Color4.Black;

    public override void Render(ObjectRenderer objectRenderer)
    {
        objectRenderer.Render(this);
    }
}
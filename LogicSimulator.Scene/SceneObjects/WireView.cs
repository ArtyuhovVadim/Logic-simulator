using LogicSimulator.Core.LogicComponents;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class WireView : BaseSceneObject
{
    public Wire Model { get; set; }

    public Vector2 FirstPoint { get; set; } = Vector2.Zero;

    public Vector2 SecondPoint { get; set; } = Vector2.Zero;

    protected override void OnInitialize(Scene2D scene)
    {
    }

    public override void StartDrag(Vector2 pos)
    {
    }

    public override void Drag(Vector2 pos)
    {
    }

    public override void EndDrag()
    {
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        return false;
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        return GeometryRelation.Unknown;
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var brush = new SolidColorBrush(renderTarget, Color4.Black);

        renderTarget.DrawLine(FirstPoint, SecondPoint, brush, 3f);
    }

    protected override void OnRenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush,
        StrokeStyle selectionStyle)
    {
    }
}
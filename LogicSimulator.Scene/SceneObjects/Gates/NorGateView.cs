using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents.Gates;
using LogicSimulator.Scene.SceneObjects.Gates.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects.Gates;

public class NorGateView : BaseGateView<NorGate>
{
    protected static readonly Resource NorGateGeometryResource = ResourceCache.Register((scene, obj) =>
    {
        var gate = (NorGateView)obj;
        var width = gate.Width;
        var height = gate.Height * 0.8f;
        var location = gate.Location + new Vector2(0, gate.Height * 0.1f);

        var sink = scene.ResourceFactory.BeginPathGeometry();

        sink.BeginFigure(location, FigureBegin.Filled);

        sink.AddBezier(new BezierSegment
        {
            Point1 = location,
            Point2 = location + new Vector2(width * 0.6666666f, 0),
            Point3 = location + new Vector2(width, height / 2)
        });

        sink.AddBezier(new BezierSegment
        {
            Point1 = location + new Vector2(width, height / 2),
            Point2 = location + new Vector2(width * 0.6666666f, height),
            Point3 = location + new Vector2(0, height)
        });

        sink.AddBezier(new BezierSegment
        {
            Point1 = location + new Vector2(0, height),
            Point2 = location + new Vector2(width * 0.3333333f, height * 0.5f),
            Point3 = location
        });

        sink.EndFigure(FigureEnd.Closed);

        return scene.ResourceFactory.EndPathGeometry();
    });

    protected override Resource GateGeometryResource => NorGateGeometryResource;

    public NorGateView(NorGate model) : base(model)
    {
        Width = 75f;
        Height = 75f;
    }

    protected override void OnInitialize(Scene2D scene)
    {
        base.OnInitialize(scene);
        InitializeResource(GateGeometryResource);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
        var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, scene);
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, NorGateGeometryResource, scene);

        var strokeWidth = 2f / scene.Scale;

        RenderPort(renderTarget, strokeBrush, Direction.Left, 0.3333333f, 25f, strokeWidth, 50f);
        RenderPort(renderTarget, strokeBrush, Direction.Left, 0.6666666f, 25f, strokeWidth, 50f);
        RenderPort(renderTarget, strokeBrush, Direction.Right, 0.5f, 25f, strokeWidth);

        renderTarget.FillGeometry(geometry, fillBrush);
        renderTarget.DrawGeometry(geometry, strokeBrush, strokeWidth);

        var ellipse = new SharpDX.Direct2D1.Ellipse(Location + new Vector2(Width, Height / 2), 4f, 4f);

        renderTarget.FillEllipse(ellipse, fillBrush);
        renderTarget.DrawEllipse(ellipse, strokeBrush, strokeWidth);

        RenderPortState(renderTarget, Model.GetPort(0).State, Direction.Left, 0.3333333f, new Vector2(-7, -8), 4f, 1f / scene.Scale);
        RenderPortState(renderTarget, Model.GetPort(1).State, Direction.Left, 0.6666666f, new Vector2(-7, -8), 4f, 1f / scene.Scale);
        RenderPortState(renderTarget, Model.GetPort(2).State, Direction.Right, 0.5f, new Vector2(7, -9), 4f, 1f / scene.Scale);
    }

    public override IEnumerable<Vector2> GetPortsPositions()
    {
        return new[]
        {
            GetPortPosition(Direction.Left, 0.3333333f, 25f),
            GetPortPosition(Direction.Left, 0.6666666f, 25f),
            GetPortPosition(Direction.Right, 0.5f, 25f),
        };
    }

    public override Port GetPort(int index)
    {
        return Model.GetPort(index);
    }
}
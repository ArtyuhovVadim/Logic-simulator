using LogicSimulator.Core.LogicComponents.Gates;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects.Gates.Base;

public class InputGateView : BaseGateView<InputGate>
{
    protected static readonly Resource InputGateGeometryResource = ResourceCache.Register((scene, obj) =>
    {
        var gate = (InputGateView)obj;
        var location = gate.Location;
        var width = gate.Width;
        var height = gate.Height;

        var sink = scene.ResourceFactory.BeginPathGeometry();

        sink.BeginFigure(location + new Vector2(width * 0.25f, 0), FigureBegin.Filled);
        sink.AddLine(location + new Vector2(width, 0));
        sink.AddLine(location + new Vector2(width, height));
        sink.AddLine(location + new Vector2(width * 0.25f, height));
        sink.AddLine(location + new Vector2(0, height / 2f));
        sink.EndFigure(FigureEnd.Closed);

        return scene.ResourceFactory.EndPathGeometry();
    });

    protected override Resource GateGeometryResource => InputGateGeometryResource;

    public InputGateView(InputGate model) : base(model)
    {
        Width = 50;
        Height = 25;
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
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, InputGateGeometryResource, scene);

        var strokeWidth = 2f / scene.Scale;

        renderTarget.FillGeometry(geometry, fillBrush);
        renderTarget.DrawGeometry(geometry, strokeBrush, strokeWidth);

        RenderPort(renderTarget, strokeBrush, Direction.Left, 0.5f, 25f, strokeWidth);
        RenderPortState(renderTarget, Model.GetPort(0).State, Direction.Left, 0.5f, new Vector2(-6, -7), 4f, 1f / scene.Scale);
    }
}
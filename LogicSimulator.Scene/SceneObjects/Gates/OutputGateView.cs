using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents.Gates;
using LogicSimulator.Scene.SceneObjects.Gates.Base;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace LogicSimulator.Scene.SceneObjects.Gates;

public class OutputGateView : BaseGateView<OutputGate>
{
    private static readonly Resource TrueTextLayoutResource = ResourceCache.Register((scene, obj) =>
    {
        var textFormat = scene.ResourceFactory.CreateTextFormat("Calibry", FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, 20f);
        var textLayout = scene.ResourceFactory.CreateTextLayout("1", textFormat);
        textFormat.Dispose();
        return textLayout;
    });

    private static readonly Resource FalseTextLayoutResource = ResourceCache.Register((scene, obj) =>
    {
        var textFormat = scene.ResourceFactory.CreateTextFormat("Calibry", FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, 20f);
        var textLayout = scene.ResourceFactory.CreateTextLayout("0", textFormat);
        textFormat.Dispose();
        return textLayout;
    });

    private static readonly Resource UndefinedTextLayoutResource = ResourceCache.Register((scene, obj) =>
    {
        var textFormat = scene.ResourceFactory.CreateTextFormat("Calibry", FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, 20f);
        var textLayout = scene.ResourceFactory.CreateTextLayout("U", textFormat);
        textFormat.Dispose();
        return textLayout;
    });

    protected static readonly Resource OutputGateGeometryResource = ResourceCache.Register((scene, obj) =>
    {
        var gate = (OutputGateView)obj;
        var location = gate.Location;
        var width = gate.Width;
        var height = gate.Height;

        var sink = scene.ResourceFactory.BeginPathGeometry();

        sink.BeginFigure(location, FigureBegin.Filled);
        sink.AddLine(location + new Vector2(width * 0.75f, 0));
        sink.AddLine(location + new Vector2(width, height / 2f));
        sink.AddLine(location + new Vector2(width * 0.75f, height));
        sink.AddLine(location + new Vector2(0, height));
        sink.EndFigure(FigureEnd.Closed);

        return scene.ResourceFactory.EndPathGeometry();
    });

    private LogicState _state;

    protected override Resource GateGeometryResource => OutputGateGeometryResource;

    public OutputGateView(OutputGate model) : base(model)
    {
        Width = 50;
        Height = 25;
    }

    protected override void OnInitialize(Scene2D scene)
    {
        base.OnInitialize(scene);
        InitializeResource(GateGeometryResource);
    }

    public override void Select()
    {
        base.Select();

        Model.State = Model.State switch
        {
            LogicState.True => LogicState.False,
            LogicState.False => LogicState.True,
            LogicState.Undefined => LogicState.False,
        };

        Model.Update();
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
        var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, scene);
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, OutputGateGeometryResource, scene);

        var layout = Model.GetPort(0).State switch
        {
            LogicState.True => ResourceCache.GetOrUpdate<TextLayout>(this, TrueTextLayoutResource, scene),
            LogicState.False => ResourceCache.GetOrUpdate<TextLayout>(this, FalseTextLayoutResource, scene),
            LogicState.Undefined => ResourceCache.GetOrUpdate<TextLayout>(this, UndefinedTextLayoutResource, scene),
            _ => null
        };

        var metrics = layout.Metrics;

        var strokeWidth = 2f / scene.Scale;

        renderTarget.FillGeometry(geometry, fillBrush);
        renderTarget.DrawGeometry(geometry, strokeBrush, strokeWidth);

        renderTarget.DrawTextLayout(Location + new Vector2(-metrics.Width / 2f + Width / 2f, -metrics.Height / 2f + Height / 2f), layout, strokeBrush);

        RenderPort(renderTarget, strokeBrush, Direction.Right, 0.5f, 25f, strokeWidth);
        RenderPortState(renderTarget, Model.GetPort(0).State, Direction.Right, 0.5f, new Vector2(6, -7), 4f, 1f / scene.Scale);
    }

    public override IEnumerable<Vector2> GetPortsPositions()
    {
        return new[]
        {
            GetPortPosition(Direction.Right, 0.5f, 25f),
        };
    }

    public override Port GetPort(int index)
    {
        return Model.GetPort(index);
    }
}
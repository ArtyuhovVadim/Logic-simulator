﻿using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents.Gates;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects.Gates.Base;

public class OutputGateView : BaseGateView<OutputGate>
{
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

        var strokeWidth = 2f / scene.Scale;

        renderTarget.FillGeometry(geometry, fillBrush);
        renderTarget.DrawGeometry(geometry, strokeBrush, strokeWidth);

        RenderPort(renderTarget, strokeBrush, Direction.Right, 0.5f, 25f, strokeWidth);
        RenderPortState(renderTarget, Model.GetPort(0).State, Direction.Right, 0.5f, new Vector2(6,-7), 4f, 1f / scene.Scale);
    }
}
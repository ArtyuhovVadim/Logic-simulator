﻿using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class SelectionRectangleRenderingComponent : BaseRenderingComponent
{
    public static readonly Resource SecantBrushResource = ResourceCache.Register((target, o) =>
        new SolidColorBrush(target, ((SelectionRectangleRenderingComponent)o).SecantColor));

    public static readonly Resource NormalBrushResource = ResourceCache.Register((target, o) =>
        new SolidColorBrush(target, ((SelectionRectangleRenderingComponent)o).NormalColor));

    private Color4 _secantColor = new(0.39f, 0.78f, 0.39f, 1f);
    private Color4 _normalColor = new(0.49f, 0.68f, 1f, 1f);
    private Vector2 _startPosition;
    private Vector2 _endPosition;

    public SelectionRectangleRenderingComponent()
    {
        IsVisible = false;
    }

    public Vector2 StartPosition
    {
        get => _startPosition;
        set
        {
            _startPosition = value;
            //TODO: RequireRender();
        }
    }

    public Vector2 EndPosition
    {
        get => _endPosition;
        set
        {
            _endPosition = value;
            //TODO: RequireRender();
        }
    }

    public Color4 SecantColor
    {
        get => _secantColor;
        set
        {
            _secantColor = value;
            ResourceCache.RequestUpdate(this, SecantBrushResource);
        }
    }

    public Color4 NormalColor
    {
        get => _normalColor;
        set
        {
            _normalColor = value;
            ResourceCache.RequestUpdate(this, NormalBrushResource);
        }
    }

    public bool IsSecant { get; set; }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var brush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, IsSecant ? SecantBrushResource : NormalBrushResource, renderTarget);

        var location = StartPosition;
        var size = EndPosition - StartPosition;

        renderTarget.DrawRectangle(new RectangleF { Location = location, Width = size.X, Height = size.Y }, brush, 1f / scene.Scale);
    }
}
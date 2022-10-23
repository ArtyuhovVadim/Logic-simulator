﻿using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class RoundedRectangle : EditableSceneObject
{
    private static readonly Resource FillBrushResource = ResourceCache.Register((target, o) => new SolidColorBrush(target, ((RoundedRectangle)o).FillColor));

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((target, o) => new SolidColorBrush(target, ((RoundedRectangle)o).StrokeColor));

    private static readonly Resource RectangleGeometryResource = ResourceCache.Register((target, o) =>
    {
        var rect = (RoundedRectangle)o;

        var rectStruct = new SharpDX.Direct2D1.RoundedRectangle()
        {
            Rect = new RectangleF(rect.Location.X, rect.Location.Y, rect.Width, rect.Height),
            RadiusX = rect._radiusX,
            RadiusY = rect._radiusY
        };

        return new RoundedRectangleGeometry(target.Factory, rectStruct);
    });

    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;
    private Vector2 _location = Vector2.Zero;
    private float _width;
    private float _height;
    private float _radiusX;
    private float _radiusY;
    private float _strokeThickness = 1f;
    private bool _isFilled = true;

    private Vector2 _startDragPosition = Vector2.Zero;
    private Vector2 _startDragLocation = Vector2.Zero;

    private static readonly AbstractNode[] AbstractNodes =
    {
        new Node<RoundedRectangle>(o => o.Location, (o, p)=>
        {
            o.Width += (o.Location - p).X;
            o.Height += (o.Location - p).Y;
            o.Location = p;
        }),
        new Node<RoundedRectangle>(o => o.Location + new Vector2(o.Width, 0), (o, p) =>
        {
            o.Width = p.X - o.Location.X;
            o.Height = o.Location.Y + o.Height - p.Y;
            o.Location = new Vector2(o.Location.X, p.Y);
        }),
        new Node<RoundedRectangle>(o => o.Location + new Vector2(o.Width, o.Height), (o, p) =>
        {
            var size = p - o.Location;
            o.Width = size.X;
            o.Height = size.Y;
        }),
        new Node<RoundedRectangle>(o => o.Location + new Vector2(0, o.Height), (o, p) =>
        {
            o.Width = o.Location.X + o.Width- p.X;
            o.Height = p.Y - o.Location.Y;
            o.Location = new Vector2(p.X, o.Location.Y);
        }),
        new Node<RoundedRectangle>(o => o.Location + new Vector2(o.RadiusX, o.RadiusY), (o, p) =>
        {
            var radius =  p - o.Location;

            o.RadiusX = radius.X;
            o.RadiusY = radius.Y;
        }, false)
    };

    public override AbstractNode[] Nodes => AbstractNodes;

    [Editable]
    public Vector2 Location
    {
        get => _location;
        set => SetAndUpdateResource(ref _location, value, RectangleGeometryResource);
    }

    [Editable]
    public float Width
    {
        get => _width;
        set => SetAndUpdateResource(ref _width, value, RectangleGeometryResource);
    }

    [Editable]
    public float Height
    {
        get => _height;
        set => SetAndUpdateResource(ref _height, value, RectangleGeometryResource);
    }

    [Editable]
    public float RadiusX
    {
        get => _radiusX;
        set => SetAndUpdateResource(ref _radiusX, value, RectangleGeometryResource);
    }

    [Editable]
    public float RadiusY
    {
        get => _radiusY;
        set => SetAndUpdateResource(ref _radiusY, value, RectangleGeometryResource);
    }

    [Editable]
    public Color4 FillColor
    {
        get => _fillColor;
        set => SetAndUpdateResource(ref _fillColor, value, FillBrushResource);
    }

    [Editable]
    public Color4 StrokeColor
    {
        get => _strokeColor;
        set => SetAndUpdateResource(ref _strokeColor, value, StrokeBrushResource);
    }

    [Editable]
    public float StrokeThickness
    {
        get => _strokeThickness;
        set => SetAndRequestRender(ref _strokeThickness, value);
    }

    [Editable]
    public bool IsFilled
    {
        get => _isFilled;
        set => SetAndRequestRender(ref _isFilled, value);
    }

    public override void StartDrag(Vector2 pos)
    {
        IsDragging = true;

        _startDragPosition = pos;
        _startDragLocation = Location;
    }

    public override void Drag(Vector2 pos)
    {
        Location = _startDragLocation - _startDragPosition + pos;
    }

    public override void EndDrag()
    {
        IsDragging = false;
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<RoundedRectangleGeometry>(this, RectangleGeometryResource);

        return IsFilled ? geometry.FillContainsPoint(pos, matrix, tolerance) :
                            geometry.StrokeContainsPoint(pos, StrokeThickness, null, matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<RoundedRectangleGeometry>(this, RectangleGeometryResource);

        return geometry.Compare(rectGeometry, matrix, tolerance);
    }

    public override void Render(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, renderTarget);
        var geometry = ResourceCache.GetOrUpdate<RoundedRectangleGeometry>(this, RectangleGeometryResource, renderTarget);

        if (IsFilled)
        {
            var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, renderTarget);

            renderTarget.FillGeometry(geometry, fillBrush);
        }

        renderTarget.DrawGeometry(geometry, strokeBrush, StrokeThickness / scene.Scale);
    }

    public override void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<RoundedRectangleGeometry>(this, RectangleGeometryResource, renderTarget);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
    }
}
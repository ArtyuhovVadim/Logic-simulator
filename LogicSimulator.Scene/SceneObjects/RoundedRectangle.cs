﻿using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class RoundedRectangle : EditableSceneObject
{
    private static readonly Resource FillBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((RoundedRectangle)obj).FillColor));

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((RoundedRectangle)obj).StrokeColor));

    private static readonly Resource RectangleGeometryResource = ResourceCache.Register((scene, obj) =>
    {
        var rect = (RoundedRectangle)obj;

        var rectStruct = new SharpDX.Direct2D1.RoundedRectangle
        {
            Rect = new RectangleF(0, 0, rect.Width, rect.Height),
            RadiusX = rect._radiusX,
            RadiusY = rect._radiusY
        };

        return scene.ResourceFactory.CreateRoundedRectangleGeometry(rectStruct);
    });

    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;
    private float _width;
    private float _height;
    private float _radiusX;
    private float _radiusY;
    private float _strokeThickness = 1f;
    private bool _isFilled = true;

    private static readonly AbstractNode[] AbstractNodes =
    {
        new Node<RoundedRectangle>(o => o.LocalToWorldSpace(Vector2.Zero), (o, p)=>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = p;
            o.Width -= localPos.X;
            o.Height -= localPos.Y;
        }),
        new Node<RoundedRectangle>(o => o.LocalToWorldSpace(new Vector2(o.Width, 0)), (o, p) =>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = o.LocalToWorldSpace(new Vector2(0, localPos.Y));
            o.Width = localPos.X;
            o.Height -= localPos.Y;
        }),
        new Node<RoundedRectangle>(o => o.LocalToWorldSpace(new Vector2(o.Width, o.Height)), (o, p) =>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Width = localPos.X;
            o.Height = localPos.Y;
        }),
        new Node<RoundedRectangle>(o => o.LocalToWorldSpace(new Vector2(0, o.Height)), (o, p) =>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = o.LocalToWorldSpace(new Vector2(localPos.X, 0));
            o.Width -= localPos.X;
            o.Height = localPos.Y;
        }),
        new Node<RoundedRectangle>(o => o.LocalToWorldSpace(new Vector2(o.RadiusX, o.RadiusY)), (o, p) =>
        {
            var radius =  o.WorldToLocalSpace(p);

            o.RadiusX = radius.X;
            o.RadiusY = radius.Y;
        }, false)
    };

    public override AbstractNode[] Nodes => AbstractNodes;

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

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(RectangleGeometryResource);
        InitializeResource(StrokeBrushResource);
        InitializeResource(FillBrushResource);
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<RoundedRectangleGeometry>(this, RectangleGeometryResource);

        return IsFilled ? geometry.FillContainsPoint(pos, TransformMatrix * matrix, tolerance) :
                            geometry.StrokeContainsPoint(pos, StrokeThickness, null, TransformMatrix * matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<RoundedRectangleGeometry>(this, RectangleGeometryResource);

        return geometry.Compare(rectGeometry, Matrix3x2.Invert(TransformMatrix) * matrix, tolerance);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
        var geometry = ResourceCache.GetOrUpdate<RoundedRectangleGeometry>(this, RectangleGeometryResource, scene);

        if (IsFilled)
        {
            var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, scene);

            renderTarget.FillGeometry(geometry, fillBrush);
        }

        renderTarget.DrawGeometry(geometry, strokeBrush, StrokeThickness / scene.Scale);
    }

    protected override void OnRenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush,
        StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<RoundedRectangleGeometry>(this, RectangleGeometryResource, scene);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
    }
}
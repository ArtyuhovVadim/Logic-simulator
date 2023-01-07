using System;
using System.DirectoryServices;
using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using Matrix3x2 = SharpDX.Matrix3x2;
using Vector2 = SharpDX.Vector2;

namespace LogicSimulator.Scene.SceneObjects;

public class Rectangle : EditableSceneObject
{
    private static readonly Resource FillBrushResource = ResourceCache.Register((scene, obj) => scene.ResourceFactory.CreateSolidColorBrush(((Rectangle)obj).FillColor));

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) => scene.ResourceFactory.CreateSolidColorBrush(((Rectangle)obj).StrokeColor));

    private static readonly Resource RectangleGeometryResource = ResourceCache.Register((scene, obj) =>
    {
        var rect = (Rectangle)obj;

        return scene.ResourceFactory.CreateRectangleGeometry(new RectangleF(0, 0, rect.Width, rect.Height));
    });

    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;
    private float _width;
    private float _height;
    private float _strokeThickness = 1f;
    private bool _isFilled = true;

    private static readonly AbstractNode[] AbstractNodes =
    {
        new Node<Rectangle>(o => o.Location, (o, p)=>
        {
            var diff = o.Location - p;

            diff = diff.RotateRelative(Utils.RotationToInt(o.Rotation), Vector2.Zero)
                   * (o.Rotation is Rotation.Degrees180 or Rotation.Degrees0 ? 1 : -1);

            o.Width += diff.X;
            o.Height += diff.Y;
            o.Location = p;
        }),
        //new Node<Rectangle>(o => Vector2.Zero.Transform(o.TransformMatrix), (o, p)=>
        //{
        //    var a = p;
        //    var b = p.Transform(o.TransformMatrix);
        //    var c = p.InvertAndTransform(o.TransformMatrix);
        //    //p = p.InvertAndTransform(o.TransformMatrix);
        //
        //    var d = c.RotateRelative(Utils.RotationToInt(o.Rotation), Vector2.Zero);
        //
        //
        //
        //    if (p.InvertAndTransform(o.TransformMatrix).Length() > 0)
        //    {
        //
        //    }
        //
        //
        //    o.Location += d;
        //    o.Width -= d.X;
        //    o.Height -= d.Y;
        //
        //    //o.Width += p.X;
        //    //o.Height += p.Y;
        //    //o.Location -= p;
        //}),
        new Node<Rectangle>(o => new Vector2(o.Width, 0).Transform(o.TransformMatrix), (o, p) =>
        {
            p = p.InvertAndTransform(o.TransformMatrix);

            o.Width = p.X;
            o.Height -= p.Y;
            o.Location += new Vector2(0, p.Y);
        }),
        new Node<Rectangle>(o => new Vector2(o.Width, o.Height).Transform(o.TransformMatrix), (o, p) =>
        {
            p = p.InvertAndTransform(o.TransformMatrix);

            o.Width = p.X;
            o.Height = p.Y;
        }),
        new Node<Rectangle>(o => new Vector2(0, o.Height).Transform(o.TransformMatrix), (o, p) =>
        {
            p = p.InvertAndTransform(o.TransformMatrix);

            o.Height = p.Y;
            o.Width -= p.X;
            o.Location += new Vector2(p.X, 0);
        })
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
        var geometry = ResourceCache.GetCached<RectangleGeometry>(this, RectangleGeometryResource);

        return IsFilled ? geometry.FillContainsPoint(pos, TransformMatrix * matrix, tolerance) :
                            geometry.StrokeContainsPoint(pos, StrokeThickness, null, TransformMatrix * matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<RectangleGeometry>(this, RectangleGeometryResource);

        return geometry.Compare(rectGeometry, Matrix3x2.Invert(TransformMatrix) * matrix, tolerance);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
        var geometry = ResourceCache.GetOrUpdate<RectangleGeometry>(this, RectangleGeometryResource, scene);

        if (IsFilled)
        {
            var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, scene);

            renderTarget.FillGeometry(geometry, fillBrush);
        }

        renderTarget.DrawGeometry(geometry, strokeBrush, StrokeThickness / scene.Scale);

        var green = new SolidColorBrush(renderTarget, new Color4(0, 1, 0, 1));
        var blue = new SolidColorBrush(renderTarget, new Color4(0, 0, 1, 1));

        //renderTarget.DrawLine(new Vector2(0, 0), new RawVector2(Math.Sign(Location.X) * 50, 0), green, 3);
        //renderTarget.DrawLine(new Vector2(0, 0), new RawVector2(0, Math.Sign(Location.Y) * 50), blue, 3);
        renderTarget.DrawLine(new Vector2(0, 0), Vector2.Normalize(Location) * 100, blue, 3);
    }

    protected override void OnRenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<RectangleGeometry>(this, RectangleGeometryResource, scene);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
    }

    public override void Rotate(Vector2 offset)
    {
        //Location = (Location + new Vector2(0, Height)).RotateRelative(90, offset);
        //(Width, Height) = (Height, Width);
    }
}
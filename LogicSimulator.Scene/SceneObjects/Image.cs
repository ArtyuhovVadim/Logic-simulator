using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.ComponentModel;

namespace LogicSimulator.Scene.SceneObjects;

public class Image : EditableSceneObject
{
    private static readonly Resource BitmapResource = ResourceCache.Register((scene, obj) =>
    {
        var image = (Image)obj;

        image._isBitmapValid = false;
        image.Width = 300;
        image.Height = 300;
        image.SourceAspectRatio = 0;

        var bitmap = scene.ResourceFactory.CreateBitmap(image.FilePath);

        if (bitmap is not null)
        {
            var size = bitmap.Size;

            image._isBitmapValid = true;
            image.Width = size.Width;
            image.Height = size.Height;
            image.SourceAspectRatio = size.Width / size.Height;
        }

        return bitmap;
    });

    private static readonly Resource BoundsGeometryResource = ResourceCache.Register((scene, obj) => scene.ResourceFactory.CreateRectangleGeometry(((Image)obj).Bounds));

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) => scene.ResourceFactory.CreateSolidColorBrush(((Image)obj).StrokeColor));

    private static readonly AbstractNode[] AbstractNodes =
    {
        new Node<Image>(o => o.LocalToWorldSpace(Vector2.Zero), (o, p)=>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = p;
            o.Width -= localPos.X;
            o.Height -= localPos.Y;
        }),
        new Node<Image>(o => o.LocalToWorldSpace(new Vector2(o.Width, 0)), (o, p) =>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = o.LocalToWorldSpace(new Vector2(0, localPos.Y));
            o.Width = localPos.X;
            o.Height -= localPos.Y;
        }),
        new Node<Image>(o => o.LocalToWorldSpace(new Vector2(o.Width, o.Height)), (o, p) =>
        {
            var localPos= o.WorldToLocalSpace(p);

            o.Width = localPos.X;
            o.Height = localPos.Y;
        }),
        new Node<Image>(o => o.LocalToWorldSpace(new Vector2(0, o.Height)), (o, p) =>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = o.LocalToWorldSpace(new Vector2(localPos.X, 0));
            o.Width -= localPos.X;
            o.Height = localPos.Y;
        })
    };

    private bool _isBitmapValid;

    private string _filePath = string.Empty;
    private float _width;
    private float _height;
    private Color4 _strokeColor = Color4.Black;
    private float _strokeThickness = 1f;
    private bool _isBordered;

    public override AbstractNode[] Nodes => AbstractNodes;

    private RectangleF Bounds => new(0, 0, _width, _height);

    public float SourceAspectRatio { get; private set; }

    public float CurrentAspectRatio => Width / Height;

    [Editable]
    public string FilePath
    {
        get => _filePath;
        set => SetAndUpdateResource(ref _filePath, value, BitmapResource);
    }

    [Editable]
    public float Width
    {
        get => _width;
        set => SetAndImmediatelyUpdateResource(ref _width, value, BoundsGeometryResource);
    }

    [Editable]
    public float Height
    {
        get => _height;
        set => SetAndImmediatelyUpdateResource(ref _height, value, BoundsGeometryResource);
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
    public bool IsBordered
    {
        get => _isBordered;
        set => SetAndRequestRender(ref _isBordered, value);
    }

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(BitmapResource);
        InitializeResource(BoundsGeometryResource);
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        return pos.InvertAndTransform(TransformMatrix * matrix).IsInRectangle(Bounds);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<RectangleGeometry>(this, BoundsGeometryResource);

        return geometry.Compare(rectGeometry, Matrix3x2.Invert(TransformMatrix) * matrix, tolerance);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var bitmap = ResourceCache.GetOrUpdate<Bitmap>(this, BitmapResource, scene);

        if (!_isBitmapValid)
        {
            RenderErrorMessage(scene, renderTarget);
            return;
        }

        if (_isBordered)
        {
            var brush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
            renderTarget.DrawRectangle(Bounds, brush, StrokeThickness / scene.Scale);
        }

        renderTarget.DrawBitmap(bitmap, Bounds, 1f, BitmapInterpolationMode.NearestNeighbor);
    }

    protected override void OnRenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        if (!_isBitmapValid) return;

        renderTarget.DrawRectangle(Bounds, selectionBrush, 1f / scene.Scale, selectionStyle);
    }

    private void RenderErrorMessage(Scene2D scene, RenderTarget renderTarget)
    {
        using var brush = scene.ResourceFactory.CreateSolidColorBrush(new Color4(0.8f, 0.1f, 0.1f, 1f));
        using var textBrush = scene.ResourceFactory.CreateSolidColorBrush(Color4.Black);
        using var textFormat = scene.ResourceFactory.CreateTextFormat("Times New Roman", FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, 24);

        var rect = new RectangleF(0, 0, Width, Height);

        renderTarget.FillRectangle(rect, brush);
        renderTarget.DrawText($"Can't load file with path:\n{FilePath}", textFormat, rect, textBrush, DrawTextOptions.Clip);
    }
}
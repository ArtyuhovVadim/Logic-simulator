using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Nodes;
using SharpDX;
using SharpDX.Direct2D1;
using Color = System.Windows.Media.Color;
using Geometry = SharpDX.Direct2D1.Geometry;
using RectangleGeometry = SharpDX.Direct2D1.RectangleGeometry;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;

namespace LogicSimulator.Scene.Views;

public class RoundedRectangleView : EditableSceneObjectView, IStroked
{
    public static readonly IResource FillBrushResource =
        ResourceCache.Register<RoundedRectangleView>((factory, user) => factory.CreateSolidColorBrush(user.FillColor.ToColor4()));

    public static readonly IResource StrokeBrushResource =
        ResourceCache.Register<RoundedRectangleView>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

    public static readonly IResource GeometryResource =
        ResourceCache.Register<RoundedRectangleView>((factory, user) => factory.CreateRectangleGeometry(user.Width, user.Height));

    private static readonly AbstractNode[] AbstractNodes =
    [
        new Node<RoundedRectangleView>(o => o.LocalToWorldSpace(Vector2.Zero), (o, p)=>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = p;
            o.Width -= localPos.X;
            o.Height -= localPos.Y;
        }),
        new Node<RoundedRectangleView>(o => o.LocalToWorldSpace(new Vector2(o.Width, 0)), (o, p) =>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = o.LocalToWorldSpace(new Vector2(0, localPos.Y));
            o.Width = localPos.X;
            o.Height -= localPos.Y;
        }),
        new Node<RoundedRectangleView>(o => o.LocalToWorldSpace(new Vector2(o.Width, o.Height)), (o, p) =>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Width = localPos.X;
            o.Height = localPos.Y;
        }),
        new Node<RoundedRectangleView>(o => o.LocalToWorldSpace(new Vector2(0, o.Height)), (o, p) =>
        {
            var localPos = o.WorldToLocalSpace(p);

            o.Location = o.LocalToWorldSpace(new Vector2(localPos.X, 0));
            o.Width -= localPos.X;
            o.Height = localPos.Y;
        }),
        new Node<RoundedRectangleView>(o => o.LocalToWorldSpace(new Vector2(o.RadiusX, o.RadiusY)), (o, p) =>
        {
            var radius =  o.WorldToLocalSpace(p);

            o.RadiusX = radius.X;
            o.RadiusY = radius.Y;
        }, useGridSnap: false)
    ];

    public override IEnumerable<AbstractNode> Nodes => AbstractNodes;

    #region Width

    public float Width
    {
        get => (float)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register(nameof(Width), typeof(float), typeof(RoundedRectangleView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region Height

    public float Height
    {
        get => (float)GetValue(HeightProperty);
        set => SetValue(HeightProperty, value);
    }

    public static readonly DependencyProperty HeightProperty =
        DependencyProperty.Register(nameof(Height), typeof(float), typeof(RoundedRectangleView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region RadiusX

    public float RadiusX
    {
        get => (float)GetValue(RadiusXProperty);
        set => SetValue(RadiusXProperty, value);
    }

    public static readonly DependencyProperty RadiusXProperty =
        DependencyProperty.Register(nameof(RadiusX), typeof(float), typeof(RoundedRectangleView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region RadiusY

    public float RadiusY
    {
        get => (float)GetValue(RadiusYProperty);
        set => SetValue(RadiusYProperty, value);
    }

    public static readonly DependencyProperty RadiusYProperty =
        DependencyProperty.Register(nameof(RadiusY), typeof(float), typeof(RoundedRectangleView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGeometryChanged));

    #endregion

    #region FillColor

    public Color FillColor
    {
        get => (Color)GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }

    public static readonly DependencyProperty FillColorProperty =
        DependencyProperty.Register(nameof(FillColor), typeof(Color), typeof(RoundedRectangleView), new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFillColorChanged));

    private static void OnFillColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RoundedRectangleView rectangleView) return;

        rectangleView.ThrowIfDisposed();

        rectangleView.Cache?.Update(rectangleView, FillBrushResource);

        rectangleView.MakeDirty();
    }

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(RoundedRectangleView), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

    private static void OnStrokeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RoundedRectangleView rectangleView) return;

        rectangleView.ThrowIfDisposed();

        rectangleView.Cache?.Update(rectangleView, StrokeBrushResource);

        rectangleView.MakeDirty();
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => (float)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(RoundedRectangleView), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => (StrokeThicknessType)GetValue(StrokeThicknessTypeProperty);
        set => SetValue(StrokeThicknessTypeProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessTypeProperty =
        DependencyProperty.Register(nameof(StrokeThicknessType), typeof(StrokeThicknessType), typeof(RoundedRectangleView), new FrameworkPropertyMetadata(StrokeThicknessType.Smallest, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region IsFilled

    public bool IsFilled
    {
        get => (bool)GetValue(IsFilledProperty);
        set => SetValue(IsFilledProperty, value);
    }

    public static readonly DependencyProperty IsFilledProperty =
        DependencyProperty.Register(nameof(IsFilled), typeof(bool), typeof(RoundedRectangleView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    public override bool HitTest(Vector2 pos, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<RectangleGeometry>(this, GeometryResource);

        return IsFilled ?
            geometry.FillContainsPoint(pos, TransformMatrix * worldTransform, tolerance) :
            geometry.StrokeContainsPoint(pos, this.GetStrokeThickness(), null, TransformMatrix * worldTransform, tolerance);
    }

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var rectGeometry = Cache.Get<RectangleGeometry>(this, GeometryResource);
        return rectGeometry.Compare(inputGeometry, Matrix3x2.Invert(TransformMatrix) * worldTransform, tolerance);
    }

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var rect = new RoundedRectangle
        {
            Rect = new RectangleF
            {
                Width = Width,
                Height = Height
            },
            RadiusX = RadiusX,
            RadiusY = RadiusY
        };

        if (IsFilled)
        {
            var fillBrush = Cache.Get<SolidColorBrush>(this, FillBrushResource);
            context.DrawingContext.FillRoundedRectangle(rect, fillBrush);
        }

        var strokeBrush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);

        context.DrawingContext.DrawRoundedRectangle(rect, strokeBrush, this.GetStrokeThickness(scene));
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get<SolidColorBrush>(SelectionBrushStaticResource);
        var style = Cache.Get<StrokeStyle>(SelectionStyleStaticResource);

        var rect = new RoundedRectangle
        {
            Rect = new RectangleF
            {
                Width = Width,
                Height = Height
            },
            RadiusX = RadiusX,
            RadiusY = RadiusY
        };

        context.DrawingContext.DrawRoundedRectangle(rect, brush, 1f / scene.Scale, style);
    }

    private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RoundedRectangleView rectangleView) return;

        rectangleView.ThrowIfDisposed();

        rectangleView.Cache?.Update(rectangleView, GeometryResource);

        rectangleView.MakeDirty();
    }
}
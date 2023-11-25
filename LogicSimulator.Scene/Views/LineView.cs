using System.Collections.Specialized;
using System.Windows;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using Color = System.Windows.Media.Color;
using Colors = System.Windows.Media.Colors;

namespace LogicSimulator.Scene.Views;

public class LineView : EditableSceneObjectView
{
    public static readonly IResource StrokeBrushResource =
        ResourceCache.Register<LineView>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

    public static readonly IResource GeometryResource =
        ResourceCache.Register<LineView>((factory, user) => factory.CreatePolylineGeometry(Vector2.Zero, user.Vertexes));

    public static readonly IResource StrokeStyleResource =
        ResourceCache.Register<LineView>((factory, _) => factory.CreateStrokeStyle(new StrokeStyleProperties { StartCap = CapStyle.Round, EndCap = CapStyle.Round, LineJoin = LineJoin.Round }));

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(LineView), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

    private static void OnStrokeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not LineView lineView) return;

        lineView.ThrowIfDisposed();

        lineView.Cache?.Update(lineView, StrokeBrushResource);

        lineView.MakeDirty();
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => (float)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(LineView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region Vertexes

    public IList<Vector2> Vertexes
    {
        get => (IList<Vector2>)GetValue(VertexesProperty);
        set => SetValue(VertexesProperty, value);
    }

    public static readonly DependencyProperty VertexesProperty =
        DependencyProperty.Register(nameof(Vertexes), typeof(IList<Vector2>), typeof(LineView), new PropertyMetadata(Enumerable.Empty<Vector2>(), OnVertexesPropertyChanged));

    private static void OnVertexesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not LineView lineView) return;

        if (e.OldValue is INotifyCollectionChanged oldCollection)
        {
            oldCollection.CollectionChanged -= lineView.OnVertexesCollectionChanged;
        }

        if (e.NewValue is INotifyCollectionChanged newCollection)
        {
            newCollection.CollectionChanged += lineView.OnVertexesCollectionChanged;
        }

        lineView.ThrowIfDisposed();

        lineView.Cache?.Update(lineView, GeometryResource);

        lineView.MakeDirty();
    }


    #endregion

    public override IEnumerable<AbstractNode> Nodes
    {
        get
        {
            var nodes = new AbstractNode[Vertexes.Count + 1];

            nodes[0] = new Node<LineView>(o => o.LocalToWorldSpace(Vector2.Zero), (o, p) =>
            {
                var localPos = o.WorldToLocalSpace(p);
                o.Location = p;

                for (var i = 0; i < o.Vertexes.Count; i++)
                {
                    o.Vertexes[i] -= localPos;
                }

                Cache.Update(this, GeometryResource);
                MakeDirty();
            });

            for (var i = 1; i < nodes.Length; i++)
            {
                var index = i - 1;

                nodes[i] = new Node<LineView>(o => o.LocalToWorldSpace(o.Vertexes[index]), (o, p) =>
                {
                    Vertexes[index] = o.WorldToLocalSpace(p);
                    Cache.Update(this, GeometryResource);
                    MakeDirty();
                });
            }

            return nodes;
        }
    }

    public override bool HitTest(Vector2 pos, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<PathGeometry>(this, GeometryResource);
        return geometry.StrokeContainsPoint(pos, StrokeThickness, null, TransformMatrix * worldTransform, tolerance);
    }

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<PathGeometry>(this, GeometryResource);
        return geometry.Compare(inputGeometry, Matrix3x2.Invert(TransformMatrix) * worldTransform, tolerance);
    }

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var geometry = Cache.Get<PathGeometry>(this, GeometryResource);
        var strokeBrush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);
        var strokeStyle = Cache.Get<StrokeStyle>(this, StrokeStyleResource);

        context.DrawingContext.DrawGeometry(geometry, strokeBrush, StrokeThickness, strokeStyle);
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get<SolidColorBrush>(SelectionBrushStaticResource);
        var style = Cache.Get<StrokeStyle>(SelectionStyleStaticResource);
        var geometry = Cache.Get<PathGeometry>(this, GeometryResource);

        var strokeWidth = 1f / scene.Scale;

        context.DrawingContext.DrawGeometry(geometry, brush, strokeWidth, style);
    }

    private void OnVertexesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Cache.Update(this, GeometryResource);
        MakeDirty();
    }

    protected override void Dispose(bool disposingManaged)
    {
        base.Dispose(disposingManaged);

        if (!disposingManaged) return;

        if (Vertexes is INotifyCollectionChanged collection)
        {
            collection.CollectionChanged -= OnVertexesCollectionChanged;
        }
    }
}
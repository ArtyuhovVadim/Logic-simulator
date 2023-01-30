using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class Line : EditableSceneObject
{
    private static readonly Resource PathGeometryResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreatePolylineGeometry(Vector2.Zero, ((Line)obj)._vertices));

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((Line)obj).StrokeColor));

    private static readonly Resource StrokeStyleResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateStrokeStyle(new StrokeStyleProperties { StartCap = CapStyle.Round, EndCap = CapStyle.Round, LineJoin = LineJoin.Round }));

    private Color4 _strokeColor = Color4.Black;
    private float _strokeThickness = 1f;

    private readonly List<Vector2> _vertices = new();

    public override AbstractNode[] Nodes
    {
        get
        {
            var nodes = new AbstractNode[_vertices.Count + 1];

            nodes[0] = new Node<Line>(o => o.LocalToWorldSpace(Vector2.Zero), (o, p) =>
            {
                var localPos = o.WorldToLocalSpace(p);
                o.Location = p;

                for (var i = 0; i < o._vertices.Count; i++)
                {
                    _vertices[i] -= localPos;
                }

                ResourceCache.RequestUpdate(this, PathGeometryResource);
                RequestRender();
                OnPropertyChanged(nameof(Vertexes));
            });

            for (var i = 1; i < nodes.Length; i++)
            {
                var index = i - 1;

                nodes[i] = new Node<Line>(o => o.LocalToWorldSpace(o._vertices[index]), (o, p) => o.ModifyVertex(index, o.WorldToLocalSpace(p)));
            }

            return nodes;
        }
    }

    [Editable]
    public IReadOnlyList<Vector2> Vertexes => _vertices;

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

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(PathGeometryResource);
        InitializeResource(StrokeBrushResource);
        InitializeResource(StrokeStyleResource);
    }

    public void AddVertex(Vector2 segment)
    {
        _vertices.Add(segment);
        ResourceCache.RequestUpdate(this, PathGeometryResource);
        RequestRender();
        OnPropertyChanged(nameof(Vertexes));
    }

    public bool RemoveVertex(Vector2 segment)
    {
        var result = _vertices.Remove(segment);

        if (result)
        {
            ResourceCache.RequestUpdate(this, PathGeometryResource);
            RequestRender();
            OnPropertyChanged(nameof(Vertexes));
        }

        return result;
    }

    public void InsertVertex(int index, Vector2 segment)
    {
        _vertices.Insert(index, segment);
        ResourceCache.RequestUpdate(this, PathGeometryResource);
        RequestRender();
        OnPropertyChanged(nameof(Vertexes));
    }

    public bool ModifyVertex(int index, Vector2 segment)
    {
        if (index < 0 || index >= _vertices.Count) return false;

        if (_vertices[index] == segment) return true;

        _vertices[index] = segment;
        ResourceCache.RequestUpdate(this, PathGeometryResource);
        RequestRender();
        OnPropertyChanged(nameof(Vertexes));

        return true;
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        if (_vertices.Count == 0) return false;

        var geometry = ResourceCache.GetCached<PathGeometry>(this, PathGeometryResource);

        return geometry.StrokeContainsPoint(pos, StrokeThickness, null, TransformMatrix * matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        if (_vertices.Count == 0) return GeometryRelation.Unknown;

        var geometry = ResourceCache.GetCached<PathGeometry>(this, PathGeometryResource);

        return geometry.Compare(rectGeometry, Matrix3x2.Invert(TransformMatrix) * matrix, tolerance);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        if (_vertices.Count == 0) return;

        var brush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, PathGeometryResource, scene);
        var strokeStyle = ResourceCache.GetOrUpdate<StrokeStyle>(this, StrokeStyleResource, scene);

        renderTarget.DrawGeometry(geometry, brush, StrokeThickness / scene.Scale, strokeStyle);
    }

    protected override void OnRenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush,
        StrokeStyle selectionStyle)
    {
        if (_vertices.Count == 0) return;

        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, PathGeometryResource, scene);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
    }
}
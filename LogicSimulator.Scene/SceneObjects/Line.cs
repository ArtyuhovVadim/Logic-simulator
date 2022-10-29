using System.Collections.Generic;
using System.Linq;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

//TODO: Сделать больше наконечников линии
public class Line : EditableSceneObject
{
    private static readonly Resource PathGeometryResource = ResourceCache.Register((target, o) =>
    {
        var line = (Line)o;
        var geometry = new PathGeometry(target.Factory);

        var sink = geometry.Open();
        sink.BeginFigure(line._vertices.First(), FigureBegin.Hollow);

        for (var i = 1; i < line._vertices.Count; i++)
        {
            sink.AddLine(line._vertices[i]);
        }

        sink.EndFigure(FigureEnd.Open);
        sink.Close();
        sink.Dispose();

        return geometry;
    });

    private static readonly Resource StrokeBrushResource = ResourceCache.Register((target, o) => new SolidColorBrush(target, ((Line)o).StrokeColor));

    private static readonly Resource StrokeStyleResource = ResourceCache.Register((target, o) =>
        new StrokeStyle(target.Factory, new StrokeStyleProperties { StartCap = CapStyle.Round, EndCap = CapStyle.Round, LineJoin = LineJoin.Round }));

    private Color4 _strokeColor = Color4.Black;
    private float _strokeThickness = 1f;

    private Vector2 _startDragPosition;
    private Vector2[] _starDragPositions;
    private readonly List<Vector2> _vertices = new();

    public override AbstractNode[] Nodes
    {
        get
        {
            var nodes = new AbstractNode[_vertices.Count];

            for (var i = 0; i < _vertices.Count; i++)
            {
                var index = i;

                nodes[i] = new Node<Line>(o => o._vertices[index], (o, p) => o.ModifyVertex(index, p));
            }

            return nodes;
        }
    }

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

    protected override void OnInitialize(Scene2D scene, RenderTarget renderTarget)
    {
        InitializeResource(PathGeometryResource);
        InitializeResource(StrokeBrushResource);
        InitializeResource(StrokeStyleResource);
    }

    public void AddVertex(Vector2 segment)
    {
        _vertices.Add(segment);
        ResourceCache.RequestUpdate(this, PathGeometryResource);
        OnPropertyChanged(nameof(Vertexes));
    }

    public bool RemoveVertex(Vector2 segment)
    {
        var result = _vertices.Remove(segment);

        if (result)
        {
            ResourceCache.RequestUpdate(this, PathGeometryResource);
            OnPropertyChanged(nameof(Vertexes));
        }

        return result;
    }

    public void InsertVertex(int index, Vector2 segment)
    {
        _vertices.Insert(index, segment);
        ResourceCache.RequestUpdate(this, PathGeometryResource);
        OnPropertyChanged(nameof(Vertexes));
    }

    public bool ModifyVertex(int index, Vector2 segment)
    {
        if (index < 0 || index >= _vertices.Count) return false;

        if (_vertices[index] == segment) return true;

        _vertices[index] = segment;
        ResourceCache.RequestUpdate(this, PathGeometryResource);
        OnPropertyChanged(nameof(Vertexes));

        return true;
    }

    public override void StartDrag(Vector2 pos)
    {
        IsDragging = true;

        _starDragPositions = new Vector2[_vertices.Count];

        _startDragPosition = pos;
        _vertices.CopyTo(_starDragPositions);
    }

    public override void Drag(Vector2 pos)
    {
        var isModified = false;

        for (var i = 0; i < _starDragPositions.Length; i++)
        {
            var newValue = _starDragPositions[i] - _startDragPosition + pos;

            if (newValue != _vertices[i])
            {
                _vertices[i] = newValue;
                isModified = true;
            }
        }

        if (!isModified) return;

        ResourceCache.RequestUpdate(this, PathGeometryResource);
        OnPropertyChanged(nameof(Vertexes));
    }

    public override void EndDrag()
    {
        IsDragging = false;
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        if (_vertices.Count == 0) return false;

        var geometry = ResourceCache.GetCached<PathGeometry>(this, PathGeometryResource);

        return geometry.StrokeContainsPoint(pos, StrokeThickness, null, matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        if (_vertices.Count == 0) return GeometryRelation.Unknown;

        var geometry = ResourceCache.GetCached<PathGeometry>(this, PathGeometryResource);

        return geometry.Compare(rectGeometry, matrix, tolerance);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        if (_vertices.Count == 0) return;

        var brush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, renderTarget);
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, PathGeometryResource, renderTarget);
        var strokeStyle = ResourceCache.GetOrUpdate<StrokeStyle>(this, StrokeStyleResource, renderTarget);

        renderTarget.DrawGeometry(geometry, brush, StrokeThickness / scene.Scale, strokeStyle);
    }

    public override void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        if (_vertices.Count == 0) return;

        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, PathGeometryResource, renderTarget);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
    }
}
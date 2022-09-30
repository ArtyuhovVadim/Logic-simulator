using System.Collections.Generic;
using System.Linq;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class Line : EditableSceneObject
{
    private static readonly Resource StrokeBrushResource = ResourceCache.Register((target, o) => new SolidColorBrush(target, ((Line)o).StrokeColor));

    private static readonly Resource PathGeometryResource = ResourceCache.Register((target, o) =>
    {
        var line = (Line)o;
        var geometry = new PathGeometry(target.Factory);

        var sink = geometry.Open();
        sink.BeginFigure(line._segments.First(), FigureBegin.Hollow);

        for (var i = 1; i < line._segments.Count; i++)
        {
            sink.AddLine(line._segments[i]);
        }

        sink.EndFigure(FigureEnd.Open);
        sink.Close();
        sink.Dispose();

        return geometry;
    });

    private static readonly Resource StrokeStyleResource = ResourceCache.Register((target, o) =>
        new StrokeStyle(target.Factory, new StrokeStyleProperties { StartCap = CapStyle.Round, EndCap = CapStyle.Round, LineJoin = LineJoin.Round }));

    private Color4 _strokeColor = Color4.Black;
    private float _strokeThickness = 1f;
    private readonly List<Vector2> _segments = new();

    private Vector2 _startDragPosition;
    private Vector2[] _starDragSegments;

    public override AbstractNode[] Nodes
    {
        get
        {
            var nodes = new AbstractNode[_segments.Count];

            for (var i = 0; i < _segments.Count; i++)
            {
                var index = i;

                nodes[i] = new Node<Line>(o => o._segments[index], (o, p) => o.ModifySegment(index, p));
            }

            return nodes;
        }
    }

    public IReadOnlyCollection<Vector2> Segments => _segments;

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

    public void AddSegment(Vector2 segment)
    {
        _segments.Add(segment);
        ResourceCache.RequestUpdate(this, PathGeometryResource);
    }

    public bool RemoveSegment(Vector2 segment)
    {
        var result = _segments.Remove(segment);

        if (result)
        {
            ResourceCache.RequestUpdate(this, PathGeometryResource);
        }

        return result;
    }

    public void InsertSegment(int index, Vector2 segment)
    {
        _segments.Insert(index, segment);
        ResourceCache.RequestUpdate(this, PathGeometryResource);
    }

    public bool ModifySegment(int index, Vector2 segment)
    {
        if (index < 0 || index >= _segments.Count) return false;

        if (_segments[index] == segment) return true;

        _segments[index] = segment;
        ResourceCache.RequestUpdate(this, PathGeometryResource);

        return true;
    }

    public override void StartDrag(Vector2 pos)
    {
        IsDragging = true;

        _starDragSegments = new Vector2[_segments.Count];

        _startDragPosition = pos;
        _segments.CopyTo(_starDragSegments);
    }

    public override void Drag(Vector2 pos)
    {
        var flag = false;

        for (var i = 0; i < _starDragSegments.Length; i++)
        {
            var newValue = _starDragSegments[i] - _startDragPosition + pos;

            if (_segments[i] == newValue)
                continue;

            _segments[i] = newValue;

            flag = true;
        }

        if (flag)
        {
            ResourceCache.RequestUpdate(this, PathGeometryResource);
        }
    }

    public override void EndDrag()
    {
        IsDragging = false;
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        if (_segments.Count == 0) return false;

        var geometry = ResourceCache.GetCached<PathGeometry>(this, PathGeometryResource);

        return geometry.StrokeContainsPoint(pos, StrokeThickness, null, matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        if (_segments.Count == 0) return GeometryRelation.Unknown;

        var geometry = ResourceCache.GetCached<PathGeometry>(this, PathGeometryResource);

        return geometry.Compare(rectGeometry, matrix, tolerance);
    }

    public override void Render(Scene2D scene, RenderTarget renderTarget)
    {
        if (_segments.Count == 0) return;

        var brush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, renderTarget);
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, PathGeometryResource, renderTarget);
        var strokeStyle = ResourceCache.GetOrUpdate<StrokeStyle>(this, StrokeStyleResource, renderTarget);
        
        renderTarget.DrawGeometry(geometry, brush, StrokeThickness / scene.Scale, strokeStyle);
    }

    public override void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        if (_segments.Count == 0) return;

        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, PathGeometryResource, renderTarget);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
    }
}
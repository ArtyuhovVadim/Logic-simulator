using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents.Gates.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects.Gates.Base;

public abstract class BaseGateView<T> : AbstractGateView where T : BaseGate
{
    protected static readonly Resource FillBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((BaseGateView<T>)obj).FillColor));

    protected static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((BaseGateView<T>)obj).StrokeColor));

    protected abstract Resource GateGeometryResource { get; }

    private Vector2 _startDragPosition;
    private Vector2 _startDragLocation;

    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;
    private Vector2 _location = Vector2.Zero;
    private float _width = 50f;
    private float _height = 50f;

    public T Model { get; private set; }

    public Vector2 Location
    {
        get => _location;
        set => SetAndUpdateResource(ref _location, value, GateGeometryResource);
    }

    public Color4 FillColor
    {
        get => _fillColor;
        set => SetAndUpdateResource(ref _fillColor, value, FillBrushResource);
    }

    public Color4 StrokeColor
    {
        get => _strokeColor;
        set => SetAndUpdateResource(ref _strokeColor, value, StrokeBrushResource);
    }

    public float Width
    {
        get => _width;
        set => SetAndUpdateResource(ref _width, value, GateGeometryResource);
    }

    public float Height
    {
        get => _height;
        set => SetAndUpdateResource(ref _height, value, GateGeometryResource);
    }

    protected BaseGateView(T model)
    {
        Model = model;
    }

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(FillBrushResource);
        InitializeResource(StrokeBrushResource);
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

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = GetOrUpdateResource<PathGeometry>(GateGeometryResource);

        return geometry.Compare(rectGeometry, matrix, tolerance);
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = GetOrUpdateResource<PathGeometry>(GateGeometryResource);

        return geometry.FillContainsPoint(pos, matrix, tolerance);
    }

    public override void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<PathGeometry>(this, GateGeometryResource, scene);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
    }

    //TODO: Переписать
    protected void RenderPortState(RenderTarget renderTarget, LogicState state, Direction direction,
        float relativePos, Vector2 offset, float size, float strokeWidth)
    {
        var trueSignalBrush = new SolidColorBrush(renderTarget, new Color4(0, 1, 0, 1));
        var falseSignalBrush = new SolidColorBrush(renderTarget, new Color4(1, 0, 0, 1));
        var unknownSignalBrush = new SolidColorBrush(renderTarget, new Color4(0, 0, 1, 1));
        var strokeBrush = new SolidColorBrush(renderTarget, Color4.Black);

        var brush = state switch
        {
            LogicState.True => trueSignalBrush,
            LogicState.False => falseSignalBrush,
            LogicState.Undefined => unknownSignalBrush
        };

        var pos = direction switch
        {
            Direction.Up => new Vector2(Width * relativePos, 0),
            Direction.Down => new Vector2(Width * relativePos, Height),
            Direction.Right => new Vector2(Width, Height * relativePos),
            Direction.Left => new Vector2(0, Height * relativePos),
        };

        pos += Location + offset;

        var rect = pos.RectangleRelativePointAsCenter(size);

        renderTarget.FillRectangle(rect, brush);
        renderTarget.DrawRectangle(rect, strokeBrush, strokeWidth);

        trueSignalBrush.Dispose();
        falseSignalBrush.Dispose();
        unknownSignalBrush.Dispose();
        strokeBrush.Dispose();
    }

    protected void RenderPort(RenderTarget renderTarget, SolidColorBrush brush, Direction direction, float relativePos, float portLength, float strokeWidth, float additionalLength = 0f)
    {
        var pos = Vector2.Zero;
        var dir = Vector2.Zero;

        switch (direction)
        {
            case Direction.Up:
                pos = new Vector2(Width * relativePos, 0);
                dir = new Vector2(0, -1);
                break;
            case Direction.Down:
                pos = new Vector2(Width * relativePos, Height);
                dir = new Vector2(0, 1);
                break;
            case Direction.Right:
                pos = new Vector2(Width, Height * relativePos);
                dir = new Vector2(1, 0);
                break;
            case Direction.Left:
                pos = new Vector2(0, Height * relativePos);
                dir = new Vector2(-1, 0);
                break;
        }

        pos += Location;

        renderTarget.DrawLine(pos + dir * -additionalLength, pos + dir * portLength, brush, strokeWidth);
    }

    protected Vector2 GetPortPosition(Direction direction, float relativePos, float portLength)
    {
        var pos = Vector2.Zero;
        var dir = Vector2.Zero;

        switch (direction)
        {
            case Direction.Up:
                pos = new Vector2(Width * relativePos, 0);
                dir = new Vector2(0, -1);
                break;
            case Direction.Down:
                pos = new Vector2(Width * relativePos, Height);
                dir = new Vector2(0, 1);
                break;
            case Direction.Right:
                pos = new Vector2(Width, Height * relativePos);
                dir = new Vector2(1, 0);
                break;
            case Direction.Left:
                pos = new Vector2(0, Height * relativePos);
                dir = new Vector2(-1, 0);
                break;
        }

        pos += Location;

        return pos + dir * portLength;
    }
}
using System;
using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents.Gates;
using LogicSimulator.Core.LogicComponents.Gates.Base;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects.Gates.Base;

public class InputGateView : BaseGateView<InputGate>
{
    public InputGateView(InputGate model) : base(model) { }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
        var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, scene);

        renderTarget.FillRectangle(boundsBox, fillBrush);
        renderTarget.DrawRectangle(boundsBox, strokeBrush, 3f);

        var port1Pos1 = new Vector2(0, 0.5f * DefaultGateHeight) + Location;
        var port1Pos2 = new Vector2(-DefaultPortLength, 0.5f * DefaultGateHeight) + Location;

        renderTarget.DrawLine(port1Pos1, port1Pos2, strokeBrush, 3f);

        DrawPortSignalState(renderTarget, port1Pos1, Model.GetPort(0).State);
    }
}

public class OutputGateView : BaseGateView<OutputGate>
{
    public override void Select()
    {
        base.Select();

        Model.State = Model.State switch
        {
            LogicState.True => LogicState.False,
            LogicState.False => LogicState.True,
            LogicState.Undefined => LogicState.False,
        };

        Model.Update();
    }

    public OutputGateView(OutputGate model) : base(model) { }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
        var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, scene);

        renderTarget.FillRectangle(boundsBox, fillBrush);
        renderTarget.DrawRectangle(boundsBox, strokeBrush, 3f);

        var port1Pos1 = new Vector2(DefaultGateWidth, 0.5f * DefaultGateHeight) + Location;
        var port1Pos2 = new Vector2(DefaultPortLength + DefaultGateWidth, 0.5f * DefaultGateHeight) + Location;

        renderTarget.DrawLine(port1Pos1, port1Pos2, strokeBrush, 3f);

        DrawPortSignalState(renderTarget, port1Pos1, Model.GetPort(0).State);
    }
}

public class NorGateView : BaseGateView<NorGate>
{
    public NorGateView(NorGate model) : base(model) { }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
        var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, scene);

        renderTarget.FillRectangle(boundsBox, fillBrush);
        renderTarget.DrawRectangle(boundsBox, strokeBrush, 3f);

        var port1Pos1 = new Vector2(0, 0.25f * DefaultGateHeight) + Location;
        var port1Pos2 = new Vector2(-DefaultPortLength, 0.25f * DefaultGateHeight) + Location;

        var port2Pos1 = new Vector2(0, 0.75f * DefaultGateHeight) + Location;
        var port2Pos2 = new Vector2(-DefaultPortLength, 0.75f * DefaultGateHeight) + Location;

        var port3Pos1 = new Vector2(DefaultGateWidth, 0.5f * DefaultGateHeight) + Location;
        var port3Pos2 = new Vector2(DefaultPortLength + DefaultGateWidth, 0.5f * DefaultGateHeight) + Location;

        renderTarget.DrawLine(port1Pos1, port1Pos2, strokeBrush, 3f);
        renderTarget.DrawLine(port2Pos1, port2Pos2, strokeBrush, 3f);
        renderTarget.DrawLine(port3Pos1, port3Pos2, strokeBrush, 3f);

        DrawPortSignalState(renderTarget, port1Pos1, Model.GetPort(0).State);
        DrawPortSignalState(renderTarget, port2Pos1, Model.GetPort(1).State);
        DrawPortSignalState(renderTarget, port3Pos1, Model.GetPort(2).State);
    }
}

public abstract class BaseGateView<T> : BaseSceneObject where T : BaseGate
{
    protected const float DefaultGateWidth = 100f;
    protected const float DefaultGateHeight = 100f;
    protected const float DefaultPortLength = 25f;

    protected static readonly Resource FillBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((BaseGateView<T>)obj).FillColor));

    protected static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((BaseGateView<T>)obj).StrokeColor));

    protected static readonly Resource BoundsBoxResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateRectangleGeometry(((BaseGateView<T>)obj).boundsBox));

    private Vector2 _startDragPosition;
    private Vector2 _startDragLocation;

    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;
    private Vector2 _location = Vector2.Zero;

    protected RectangleF boundsBox = new(0, 0, DefaultGateWidth, DefaultGateHeight);

    public T Model { get; private set; }

    public Vector2 Location
    {
        get => _location;
        set
        {
            boundsBox.Location = value;
            boundsBox.Width = DefaultGateWidth;
            boundsBox.Height = DefaultGateHeight;

            SetAndUpdateResource(ref _location, value, BoundsBoxResource);
        }
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

    protected BaseGateView(T model)
    {
        Model = model;
    }

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(FillBrushResource);
        InitializeResource(StrokeBrushResource);
        InitializeResource(BoundsBoxResource);
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
        var geometry = GetOrUpdateResource<RectangleGeometry>(BoundsBoxResource);

        return geometry.Compare(rectGeometry, matrix, tolerance);
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = GetOrUpdateResource<RectangleGeometry>(BoundsBoxResource);

        return geometry.FillContainsPoint(pos, matrix, tolerance);
    }

    public override void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<RectangleGeometry>(this, BoundsBoxResource, scene);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
    }

    protected void DrawPortSignalState(RenderTarget renderTarget, Vector2 pos, LogicState state)
    {
        var trueSignalBrush = new SolidColorBrush(renderTarget, new Color4(0, 1, 0, 1));
        var falseSignalBrush = new SolidColorBrush(renderTarget, new Color4(1, 0, 0, 1));
        var unknownSignalBrush = new SolidColorBrush(renderTarget, new Color4(0, 0, 1, 1));

        var strokeBrush = new SolidColorBrush(renderTarget, Color4.Black);

        var brush = state switch
        {
            LogicState.True => trueSignalBrush,
            LogicState.False => falseSignalBrush,
            LogicState.Undefined => unknownSignalBrush,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

        var rect = pos.RectangleRelativePointAsCenter(5f);

        renderTarget.FillRectangle(rect, brush);
        renderTarget.DrawRectangle(rect, strokeBrush);

        trueSignalBrush.Dispose();
        falseSignalBrush.Dispose();
        unknownSignalBrush.Dispose();
        strokeBrush.Dispose();
    }
}
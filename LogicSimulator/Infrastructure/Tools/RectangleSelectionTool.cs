using System.Windows;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;
using LogicSimulator.Scene.Layers;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Infrastructure.Tools;

public class RectangleSelectionTool : BaseTool
{
    private bool _isSelectionChanged;

    #region StartPosition

    public Vector2 StartPosition
    {
        get => (Vector2)GetValue(StartPositionProperty);
        set => SetValue(StartPositionProperty, value);
    }

    public static readonly DependencyProperty StartPositionProperty =
        DependencyProperty.Register(nameof(StartPosition), typeof(Vector2), typeof(RectangleSelectionTool), new PropertyMetadata(default(Vector2)));

    #endregion

    #region EndPosition

    public Vector2 EndPosition
    {
        get => (Vector2)GetValue(EndPositionProperty);
        set => SetValue(EndPositionProperty, value);
    }

    public static readonly DependencyProperty EndPositionProperty =
        DependencyProperty.Register(nameof(EndPosition), typeof(Vector2), typeof(RectangleSelectionTool), new PropertyMetadata(default(Vector2)));

    #endregion

    #region ManySelectionKey

    public Key ManySelectionKey
    {
        get => (Key)GetValue(ManySelectionKeyProperty);
        set => SetValue(ManySelectionKeyProperty, value);
    }

    public static readonly DependencyProperty ManySelectionKeyProperty =
        DependencyProperty.Register(nameof(ManySelectionKey), typeof(Key), typeof(RectangleSelectionTool), new PropertyMetadata(Key.LeftShift));

    #endregion

    #region IsSelectionStarted

    public bool IsSelectionStarted
    {
        get => (bool)GetValue(IsSelectionStartedProperty);
        set => SetValue(IsSelectionStartedProperty, value);
    }

    public static readonly DependencyProperty IsSelectionStartedProperty =
        DependencyProperty.Register(nameof(IsSelectionStarted), typeof(bool), typeof(RectangleSelectionTool), new PropertyMetadata(default(bool)));

    #endregion

    #region ObjectsLayer

    public ObjectsLayer ObjectsLayer
    {
        get => (ObjectsLayer)GetValue(ObjectsLayerProperty);
        set => SetValue(ObjectsLayerProperty, value);
    }

    public static readonly DependencyProperty ObjectsLayerProperty =
        DependencyProperty.Register(nameof(ObjectsLayer), typeof(ObjectsLayer), typeof(RectangleSelectionTool), new PropertyMetadata(default(ObjectsLayer)));

    #endregion

    #region Geometry

    public RectangleGeometry Geometry
    {
        get => (RectangleGeometry)GetValue(GeometryProperty);
        set => SetValue(GeometryProperty, value);
    }

    public static readonly DependencyProperty GeometryProperty =
        DependencyProperty.Register(nameof(Geometry), typeof(RectangleGeometry), typeof(RectangleSelectionTool), new PropertyMetadata(default(RectangleGeometry)));

    #endregion

    #region ObjectSelectedCommand

    public ICommand ObjectSelectedCommand
    {
        get => (ICommand)GetValue(ObjectSelectedCommandProperty);
        set => SetValue(ObjectSelectedCommandProperty, value);
    }

    public static readonly DependencyProperty ObjectSelectedCommandProperty =
        DependencyProperty.Register(nameof(ObjectSelectedCommand), typeof(ICommand), typeof(RectangleSelectionTool), new PropertyMetadata(default(ICommand)));

    #endregion

    protected override void OnKeyDown(Scene2D scene, KeyEventArgs args, Vector2 pos)
    {
        if (args.Key != CancelKey) return;

        ToolsController.SwitchToDefaultTool();
    }

    protected override void OnMouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        StartPosition = pos;
        EndPosition = pos;
        IsSelectionStarted = true;
    }

    protected override void OnMouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        EndPosition = pos;
    }

    protected override void OnMouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        var isManySelectionKeyDown = Keyboard.IsKeyDown(ManySelectionKey);

        foreach (var sceneObject in ObjectsLayer.Objects.Select(ObjectsLayer.GetViewFromItem))
        {
            if (!isManySelectionKeyDown)
            {
                sceneObject!.Unselect();
                _isSelectionChanged = true;
            }

            var compareResult = sceneObject!.HitTest(Geometry, Matrix3x2.Identity);

            if (compareResult is GeometryRelation.Disjoint or GeometryRelation.Unknown)
                continue;

            switch (EndPosition.X < StartPosition.X)
            {
                case true when compareResult is GeometryRelation.IsContained or GeometryRelation.Overlap:
                case false when compareResult is GeometryRelation.IsContained:
                    sceneObject.Select();
                    _isSelectionChanged = true;
                    break;
            }
        }

        if (_isSelectionChanged)
            ObjectSelectedCommand?.Execute(null);

        if (ActivatedFromOtherTool)
            ToolsController.SwitchToDefaultTool();
        else
            IsSelectionStarted = false;
    }

    protected override void OnDeactivated() => IsSelectionStarted = false;

    protected override Freezable CreateInstanceCore() => new RectangleSelectionTool();
}
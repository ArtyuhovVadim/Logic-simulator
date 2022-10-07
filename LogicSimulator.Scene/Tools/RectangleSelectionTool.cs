using System;
using System.Windows.Input;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Tools;

public class RectangleSelectionTool : BaseTool
{
    private bool _isStartPositionInit;
    private bool _isSelectionChanged;

    private SelectionRectangleRenderingComponent _component;

    public event Action SelectionChanged;

    public Vector2 StartPosition { get; private set; }
    public Vector2 EndPosition { get; private set; }

    public bool IsSecant { get; private set; }

    public Key ManySelectionKey { get; set; } = Key.LeftShift;

    protected override void OnActivated(Scene2D scene)
    {
        _component = scene.GetComponent<SelectionRectangleRenderingComponent>();
    }

    public override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        pos = pos.Transform(scene.Transform);

        if (!_isStartPositionInit)
        {
            StartPosition = pos;
            _component.StartPosition = pos;
            _isStartPositionInit = true;
            _component.IsVisible = true;
        }

        EndPosition = pos;
        _component.EndPosition = pos;

        IsSecant = EndPosition.X < StartPosition.X;
        _component.IsSecant = IsSecant;
    }

    public override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        var size = EndPosition - StartPosition;

        using var selectionGeometry =
            new RectangleGeometry(scene.RenderTarget.Factory, new RectangleF { Location = StartPosition, Width = size.X, Height = size.Y });

        var isManySelectionKeyDown = Keyboard.IsKeyDown(ManySelectionKey);

        foreach (var sceneObject in scene.Objects)
        {
            if (!isManySelectionKeyDown)
            {
                sceneObject.Unselect();
                _isSelectionChanged = true;
            }

            var compareResult = sceneObject.CompareWithRectangle(selectionGeometry, Matrix3x2.Identity);

            if (compareResult is GeometryRelation.Disjoint or GeometryRelation.Unknown)
                continue;

            switch (IsSecant)
            {
                case true when compareResult is GeometryRelation.IsContained or GeometryRelation.Overlap:
                case false when compareResult is GeometryRelation.IsContained:
                    sceneObject.Select();
                    _isSelectionChanged = true;
                    break;
            }
        }

        if (_isSelectionChanged)
            SelectionChanged?.Invoke();

        scene.SwitchTool<SelectionTool>();
    }

    protected override void OnDeactivated(Scene2D scene)
    {
        _isStartPositionInit = false;
        _component.IsVisible = false;
    }
}
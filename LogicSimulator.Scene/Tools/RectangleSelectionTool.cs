using System.Windows.Input;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Tools;

public class RectangleSelectionTool : BaseTool
{
    private bool _isSelectionChanged;

    private SelectionRectangleRenderingComponent _component;

    public Vector2 StartPosition { get; private set; }

    public Vector2 EndPosition { get; private set; }

    public Key ManySelectionKey { get; set; } = Key.LeftShift;

    internal override void MouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        _component = scene.GetComponent<SelectionRectangleRenderingComponent>();

        pos = pos.InvertAndTransform(scene.Transform);

        StartPosition = pos;
        EndPosition = pos;
        
        _component.StartPosition = pos;
        _component.EndPosition = pos;
        _component.IsVisible = true;
    }

    internal override void MouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        pos = pos.InvertAndTransform(scene.Transform);

        EndPosition = pos;
        _component.EndPosition = pos;
    }

    internal override void MouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        var size = EndPosition - StartPosition;

        using var selectionGeometry = scene.ResourceFactory.CreateRectangleGeometry(new RectangleF { Location = StartPosition, Width = size.X, Height = size.Y });

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
            ToolsController.OnSelectedObjectsChanged();

        ToolsController.SwitchToDefaultTool();
    }

    protected override void OnDeactivated(ToolsController toolsController)
    {
        _component.IsVisible = false;
    }
}
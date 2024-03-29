﻿using System.Windows;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;
using LogicSimulator.Scene.Layers;
using LogicSimulator.Scene.Nodes;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public class SelectionTool : BaseTool
{
    private Vector2 _startPos;

    private List<SceneObjectView> _objectsUnderCursor = null!;

    #region ObjectsLayer

    public ObjectsLayer ObjectsLayer
    {
        get => (ObjectsLayer)GetValue(ObjectsLayerProperty);
        set => SetValue(ObjectsLayerProperty, value);
    }

    public static readonly DependencyProperty ObjectsLayerProperty =
        DependencyProperty.Register(nameof(ObjectsLayer), typeof(ObjectsLayer), typeof(SelectionTool), new PropertyMetadata(default(ObjectsLayer)));

    #endregion

    #region SelectionTolerance

    public double SelectionTolerance
    {
        get => (double)GetValue(SelectionToleranceProperty);
        set => SetValue(SelectionToleranceProperty, value);
    }

    public static readonly DependencyProperty SelectionToleranceProperty =
        DependencyProperty.Register(nameof(SelectionTolerance), typeof(double), typeof(SelectionTool), new PropertyMetadata(5d));

    #endregion

    #region MultipleSelectionKey

    public Key MultipleSelectionKey
    {
        get => (Key)GetValue(MultipleSelectionKeyProperty);
        set => SetValue(MultipleSelectionKeyProperty, value);
    }

    public static readonly DependencyProperty MultipleSelectionKeyProperty =
        DependencyProperty.Register(nameof(MultipleSelectionKey), typeof(Key), typeof(SelectionTool), new PropertyMetadata(Key.LeftShift));

    #endregion

    #region DragThreshold

    public double DragThreshold
    {
        get => (double)GetValue(DragThresholdProperty);
        set => SetValue(DragThresholdProperty, value);
    }

    public static readonly DependencyProperty DragThresholdProperty =
        DependencyProperty.Register(nameof(DragThreshold), typeof(double), typeof(SelectionTool), new PropertyMetadata(5d));

    #endregion

    #region ObjectSelectedCommand

    public ICommand? ObjectSelectedCommand
    {
        get => (ICommand)GetValue(ObjectSelectedCommandProperty);
        set => SetValue(ObjectSelectedCommandProperty, value);
    }

    public static readonly DependencyProperty ObjectSelectedCommandProperty =
        DependencyProperty.Register(nameof(ObjectSelectedCommand), typeof(ICommand), typeof(SelectionTool), new PropertyMetadata(default(ICommand)));

    #endregion

    protected override void OnMouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        var isNodeThatIntersectPointExists = ObjectsLayer.Objects
            .Select(ObjectsLayer.GetViewFromItem)
            .OfType<EditableSceneObjectView>()
            .Any(obj => obj.Nodes.Any(node => obj.IsSelected && pos.IsInRectangle(node.GetLocation(obj).RectangleRelativePointAsCenter(AbstractNode.NodeSize / scene.Scale))));

        if (isNodeThatIntersectPointExists)
        {
            ToolsController.SwitchTool<NodeDragTool>(tool => tool.MouseLeftButtonDown(scene, pos));
            return;
        }

        _objectsUnderCursor = ObjectsLayer.Objects
            .Select(ObjectsLayer.GetViewFromItem)
            .Where(objView => objView is not null && objView.HitTest(pos, Matrix3x2.Identity, (float)SelectionTolerance))
            .Reverse()
            .ToList()!;

        _startPos = pos;
    }

    protected override void OnMouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if (_objectsUnderCursor.Any())
        {
            if ((pos - _startPos).Length() > DragThreshold)
                ToolsController.SwitchTool<DragTool>(tool => tool.MouseLeftButtonDown(scene, _startPos));
        }
        else
        {
            ToolsController.SwitchTool<RectangleSelectionTool>(tool => tool.MouseLeftButtonDown(scene, pos));
        }
    }

    protected override void OnMouseLeftButtonUp(Scene2D scene, Vector2 pos)
    {
        var objects = _objectsUnderCursor.ToList();

        var isMultipleSelectionKeyPressed = Keyboard.IsKeyDown(MultipleSelectionKey);

        if (objects.Count == 0)
        {
            if (!isMultipleSelectionKeyPressed)
            {
                UnselectAllObjects(ObjectsLayer);
                ObjectSelectedCommand?.Execute(null);
            }
        }
        else if (objects.Count == 1)
        {
            var obj = objects.First();

            if (obj.IsSelected) obj.Unselect();
            else
            {
                if (!isMultipleSelectionKeyPressed)
                {
                    UnselectAllObjects(ObjectsLayer);
                }

                obj.Select();
            }

            ObjectSelectedCommand?.Execute(null);
        }
        else
        {
            var selectedObjectIndex = -1;

            for (var i = 0; i < objects.Count; i++)
            {
                if (objects[i].IsSelected)
                {
                    selectedObjectIndex = i;
                    break;
                }
            }

            if (!isMultipleSelectionKeyPressed)
            {
                UnselectAllObjects(ObjectsLayer);
            }

            if (selectedObjectIndex == -1)
            {
                objects[0].Select();
            }
            else
            {
                var nextSelectedObjectIndex = selectedObjectIndex + 1 < objects.Count ? selectedObjectIndex + 1 : 0;

                objects[selectedObjectIndex].Unselect();
                objects[nextSelectedObjectIndex].Select();
            }

            ObjectSelectedCommand?.Execute(null);
        }
    }

    private static void UnselectAllObjects(ObjectsLayer layer)
    {
        foreach (var sceneObject in layer.Objects.Select(layer.GetViewFromItem))
        {
            sceneObject!.Unselect();
        }
    }

    protected override Freezable CreateInstanceCore() => new SelectionTool();
}
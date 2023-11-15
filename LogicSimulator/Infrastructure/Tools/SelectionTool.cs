using System.Windows;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;
using LogicSimulator.Scene.Layers;
using LogicSimulator.Scene.Views.Base;
using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public class SelectionTool : BaseTool
{
    private IEnumerable<SceneObjectView> _objectsUnderCursor;

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

    //TODO
    /*protected override void OnKeyDown(Scene2D scene, KeyEventArgs args, Vector2 pos)
    {
        if (args.Key == Key.Space)
        {
            pos = pos.InvertAndTransform(scene.Transform).ApplyGrid(25f);

            var sceneObjects = scene.Objects.Where(x => x.IsSelected).ToList();

            foreach (var o in sceneObjects)
            {
                o.Rotation = Utils.GetNextRotation(o.Rotation);
            }
        }
    }*/

    protected override void OnMouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        //TODO
        //if (scene.IsNodeThatIntersectPointExists(pos.InvertAndTransform(scene.Transform)))
        //{
        //    ToolsController.SwitchTool<NodeDragTool>(tool => tool.MouseLeftButtonDown(scene, pos));
        //    return;
        //}

        _objectsUnderCursor = ObjectsLayer.Objects
            .Select(ObjectsLayer.GetViewFromItem)
            .Where(objView => objView is not null && objView.HitTest(pos, Matrix3x2.Identity, (float)SelectionTolerance))
            .Reverse();
    }

    protected override void OnMouseLeftButtonDragged(Scene2D scene, Vector2 pos)
    {
        if (_objectsUnderCursor.Any())
        {
            ToolsController.SwitchTool<DragTool>(tool => tool.MouseLeftButtonDown(scene, pos));
        }
        //TODO
        //else
        //{
        //    ToolsController.SwitchTool<RectangleSelectionTool>(tool => tool.MouseLeftButtonDown(scene, pos));
        //}
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
                //TODO
                //ToolsController.OnSelectedObjectsChanged();
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

            //TODO
            //ToolsController.OnSelectedObjectsChanged();
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

            //TODO
            //ToolsController.OnSelectedObjectsChanged();
        }
    }

    private static void UnselectAllObjects(ObjectsLayer layer)
    {
        foreach (var sceneObject in layer.Objects.Select(layer.GetViewFromItem))
        {
            sceneObject.Unselect();
        }
    }

    protected override Freezable CreateInstanceCore() => new SelectionTool();
}
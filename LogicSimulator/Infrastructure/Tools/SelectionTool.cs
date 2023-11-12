using System.Windows;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;
using LogicSimulator.Scene.Layers;
using LogicSimulator.Scene.Views.Base;
using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public class SelectionTool : BaseTool
{
    #region ObjectsLayer

    public ObjectsLayer ObjectsLayer
    {
        get => (ObjectsLayer)GetValue(ObjectsLayerProperty);
        set => SetValue(ObjectsLayerProperty, value);
    }

    public static readonly DependencyProperty ObjectsLayerProperty =
        DependencyProperty.Register(nameof(ObjectsLayer), typeof(ObjectsLayer), typeof(SelectionTool), new PropertyMetadata(default(ObjectsLayer)));

    #endregion

    protected override void OnMouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        var objectsUnderCursor = new List<SceneObjectView>();

        foreach (var obj in ObjectsLayer.Objects)
        {
            var objView = ObjectsLayer.GetViewFromItem(obj);

            if (objView is null) continue;

            if (objView.HitTest(pos, scene.Transform))
            {
                objectsUnderCursor.Add(objView);
            }
        }
    }

    protected override Freezable CreateInstanceCore() => new SelectionTool();
}
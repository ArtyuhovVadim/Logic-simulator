using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.SceneObjects;
using SharpDX;
using SharpDX.Direct2D1;
using System.Linq;
using LogicSimulator.Scene.ExtensionMethods;

namespace LogicSimulator.Scene.Components;

public class NodeRenderingComponent : BaseRenderingComponent
{
    public static readonly Resource StrokeBrushResource = ResourceCache.Register((target, o) => new SolidColorBrush(target, Color4.Black));

    public static readonly Resource SelectBrushResource = ResourceCache.Register((target, o) => new SolidColorBrush(target, new Color4(1, 0, 0, 1)));

    public static readonly Resource UnselectBrushResource = ResourceCache.Register((target, o) => new SolidColorBrush(target, new Color4(0, 1, 0, 1)));

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeColor = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, renderTarget);
        var selectedColor = ResourceCache.GetOrUpdate<SolidColorBrush>(this, SelectBrushResource, renderTarget);
        var unselectedColor = ResourceCache.GetOrUpdate<SolidColorBrush>(this, UnselectBrushResource, renderTarget);

        var size = Node.NodeSize / scene.Scale;

        foreach (var sceneObject in scene.Objects.OfType<EditableSceneObject>().Where(x => x.IsSelected))
        {
            foreach (var node in sceneObject.Nodes)
            {
                var rect = node.Location.RectangleRelativePointAsCenter(size);

                renderTarget.FillRectangle(rect, node.IsSelected ? selectedColor : unselectedColor);
                renderTarget.DrawRectangle(rect, strokeColor, 1f / scene.Scale);
            }
        }
    }
}
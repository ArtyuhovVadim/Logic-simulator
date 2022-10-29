using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;
using System.Linq;
using LogicSimulator.Utils;

namespace LogicSimulator.Scene.Components;

public class NodeRenderingComponent : BaseRenderingComponent
{
    private static readonly Resource StrokeBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((NodeRenderingComponent)obj).StrokeColor));

    private static readonly Resource BackgroundBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((NodeRenderingComponent)obj).BackgroundColor));

    private Color4 _strokeColor = Color4.Black;
    private Color4 _backgroundColor = Color4.White;

    public Color4 StrokeColor
    {
        get => _strokeColor;
        set => SetAndUpdateResource(ref _strokeColor, value, StrokeBrushResource);
    }

    public Color4 BackgroundColor
    {
        get => _backgroundColor;
        set => SetAndUpdateResource(ref _backgroundColor, value, BackgroundBrushResource);
    }

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(BackgroundBrushResource);
        InitializeResource(StrokeBrushResource);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, scene);
        var backgroundBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, BackgroundBrushResource, scene);

        var size = AbstractNode.NodeSize / scene.Scale;

        foreach (var sceneObject in scene.Objects.OfType<EditableSceneObject>().Where(x => x.IsSelected))
        {
            foreach (var node in sceneObject.Nodes)
            {
                var rect = node.GetLocation(sceneObject).RectangleRelativePointAsCenter(size);

                renderTarget.FillRectangle(rect, backgroundBrush);
                renderTarget.DrawRectangle(rect, strokeBrush, 1f / scene.Scale);
            }
        }
    }
}
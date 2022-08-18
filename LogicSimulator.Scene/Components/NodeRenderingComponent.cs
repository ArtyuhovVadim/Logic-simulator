﻿using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.SceneObjects;
using SharpDX;
using SharpDX.Direct2D1;
using System.Linq;
using LogicSimulator.Scene.ExtensionMethods;

namespace LogicSimulator.Scene.Components;

public class NodeRenderingComponent : BaseRenderingComponent
{
    public static readonly Resource StrokeBrushResource = Resource.Register<NodeRenderingComponent, SolidColorBrush>(nameof(StrokeBrushResource),
        (target, o) => new SolidColorBrush(target, Color4.Black));

    public static readonly Resource SelectBrushResource = Resource.Register<NodeRenderingComponent, SolidColorBrush>(nameof(SelectBrushResource),
        (target, o) => new SolidColorBrush(target, new Color4(1, 0, 0, 1)));

    public static readonly Resource UnselectBrushResource = Resource.Register<NodeRenderingComponent, SolidColorBrush>(nameof(UnselectBrushResource),
        (target, o) => new SolidColorBrush(target, new Color4(0, 1, 0, 1)));

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeColor = GetResourceValue<SolidColorBrush>(StrokeBrushResource, renderTarget);
        var selectedColor = GetResourceValue<SolidColorBrush>(SelectBrushResource, renderTarget);
        var unselectedColor = GetResourceValue<SolidColorBrush>(UnselectBrushResource, renderTarget);

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
using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class SolidClearRenderingComponent : BaseRenderingComponent
{
    private static readonly Resource ClearBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((SolidClearRenderingComponent)obj).ClearColor));

    private Color4 _clearColor = Color4.White;

    public Color4 ClearColor
    {
        get => _clearColor;
        set => SetAndUpdateResource(ref _clearColor, value, ClearBrushResource);
    }

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(ClearBrushResource);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        renderTarget.Clear(ClearColor);
    }
}
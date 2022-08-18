using LogicSimulator.Scene.Components.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Components;

public class SolidClearRenderingComponent : BaseRenderingComponent
{
    private Color4 _clearColor = Color4.White;

    public static readonly Resource ClearColorResource = Resource.Register<SolidClearRenderingComponent, SolidColorBrush>(nameof(ClearColorResource),
            (target, o) => new SolidColorBrush(target, ((SolidClearRenderingComponent)o).ClearColor));

    public Color4 ClearColor
    {
        get => _clearColor;
        set
        {
            _clearColor = value;
            RequireUpdate(ClearColorResource);
        }
    }

    public override void Render(Scene2D scene, RenderTarget renderTarget)
    {
        renderTarget.Clear(ClearColor);
    }
}
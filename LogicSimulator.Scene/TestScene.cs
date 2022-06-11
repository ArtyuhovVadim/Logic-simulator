using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace LogicSimulator.Scene;

public class TestScene : BaseScene2D
{
    public override void Render(RenderTarget renderTarget)
    {
        renderTarget.Clear(new RawColor4(0.5f, 0.5f, 0.5f, 1f));

        using var brush = new SolidColorBrush(renderTarget, new RawColor4(1, 1, 0, 1));
        renderTarget.DrawRectangle(new RawRectangleF(200, 200, 300, 400), brush, 1);
    }
}
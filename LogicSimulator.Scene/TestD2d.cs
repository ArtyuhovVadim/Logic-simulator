using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace LogicSimulator.Scene;

public class TestD2d : D2dControl
{
    public override void Render(RenderTarget target)
    {
        target.Clear(new RawColor4(0.5f, 0.5f, 0.5f, 1));
    }
}
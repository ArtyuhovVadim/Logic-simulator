
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace LogicSimulator.Scene;

public class TestD2d : LogicSimulator.Scene.D2dControl
{
    public override void Render(RenderTarget target)
    {
        target.Clear(new RawColor4(0.5f, 0.5f, 0.5f, 1));

        var rect = new RawRectangleF(100, 100, 200, 300);

        using var geometry = new RectangleGeometry(target.Factory, rect);
        using var brush = new SolidColorBrush(target, new RawColor4(1, 0, 1, 1));

        target.FillGeometry(geometry, brush);
    }
}
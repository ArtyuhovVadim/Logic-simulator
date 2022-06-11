using SharpDX.Direct2D1;

namespace LogicSimulator.Scene;

public abstract class BaseScene2D
{
    public abstract void Render(RenderTarget renderTarget);
}
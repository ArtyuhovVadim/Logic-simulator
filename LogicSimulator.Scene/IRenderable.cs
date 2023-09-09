using LogicSimulator.Scene.DirectX;

namespace LogicSimulator.Scene;

public interface IRenderable : IDisposable
{
    bool IsDirty { get; }

    void Render(Scene2D scene, D2DContext context);
}
using LogicSimulator.Scene.DirectX;

namespace LogicSimulator.Scene;

public interface ISelectionRenderable : IRenderable
{
    bool IsSelected { get; }

    void RenderSelection(Scene2D scene, D2DContext context);
}
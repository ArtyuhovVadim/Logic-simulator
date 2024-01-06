using LogicSimulator.Scene.DirectX;

namespace LogicSimulator.Scene.Cache;

public interface IStaticResource
{
    long Id { get; }

    IDisposable Update(D2DResourceFactory factory);
}
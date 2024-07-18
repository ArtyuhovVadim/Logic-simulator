using LogicSimulator.Scene.DirectX;

namespace LogicSimulator.Scene.Cache;

public interface IStaticResource<out TResource> where TResource : class, IDisposable
{
    long Id { get; }

    TResource Update(D2DResourceFactory factory);
}
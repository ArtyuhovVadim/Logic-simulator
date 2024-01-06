using LogicSimulator.Scene.DirectX;

namespace LogicSimulator.Scene.Cache;

public interface IResource
{
    long Id { get; }

    IDisposable Update(D2DResourceFactory factory, IResourceUser user);
}
using LogicSimulator.Scene.DirectX;

namespace LogicSimulator.Scene.Cache;

public interface IResource<in TUser, out TResource> 
    where TUser : class, IResourceUser 
    where TResource : class, IDisposable
{
    long Id { get; }

    TResource Update(D2DResourceFactory factory, TUser user);
}
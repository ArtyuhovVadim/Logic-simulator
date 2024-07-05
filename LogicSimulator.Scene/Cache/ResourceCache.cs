using LogicSimulator.Scene.DirectX;

namespace LogicSimulator.Scene.Cache;

public class ResourceCache : IDisposable
{
    private readonly Dictionary<Guid, Dictionary<long, IDisposable>> _cache = [];
    private readonly Dictionary<long, IDisposable> _staticCache = [];

    private static long _lastId;

    private static long _lastStaticId;

    private readonly D2DResourceFactory _factory;

    public ResourceCache(D2DResourceFactory factory) => _factory = factory;

    public void Update<TUser, TResource>(TUser user, IResource<TUser, TResource> resource)
        where TResource : class, IDisposable
        where TUser : class, IResourceUser
    {
        if (_cache.TryGetValue(user.Id, out var resourceMap))
        {
            if (resourceMap.TryGetValue(resource.Id, out var managedResource))
            {
                managedResource.Dispose();
            }
        }
        else
        {
            _cache[user.Id] = new Dictionary<long, IDisposable>(1);
        }

        _cache[user.Id][resource.Id] = resource.Update(_factory, user);
    }

    public void UpdateStatic<TResource>(IStaticResource<TResource> resource) where TResource : class, IDisposable
    {
        if (_staticCache.TryGetValue(resource.Id, out var managedResource))
        {
            managedResource.Dispose();
        }

        _staticCache[resource.Id] = resource.Update(_factory);
    }

    public void ReleaseAll()
    {
        foreach (var (_, resourceMap) in _cache)
        {
            foreach (var (_, resource) in resourceMap)
            {
                resource.Dispose();
            }

            resourceMap.Clear();
        }

        _cache.Clear();

        foreach (var (_, resource) in _staticCache)
        {
            resource.Dispose();
        }

        _staticCache.Clear();
    }

    public void Release(IResourceUser user)
    {
        if (!_cache.TryGetValue(user.Id, out var resources)) return;

        foreach (var (_, resource) in resources)
        {
            resource.Dispose();
        }

        resources.Clear();
    }

    public TResource Get<TUser, TResource>(TUser user, IResource<TUser, TResource> resource)
        where TResource : class, IDisposable
        where TUser : class, IResourceUser
    {
        if (_cache.TryGetValue(user.Id, out var resourceMap))
        {
            if (resourceMap.TryGetValue(resource.Id, out var managedResource))
            {
                if (managedResource is TResource res1)
                    return res1;

                throw new InvalidCastException($"Can not cast resource to {typeof(TResource).Name}.");
            }
        }
        else
        {
            _cache[user.Id] = new Dictionary<long, IDisposable>(1);
        }

        var res2 = resource.Update(_factory, user);

        _cache[user.Id][resource.Id] = res2;

        return res2;
    }

    public TResource Get<TResource>(IStaticResource<TResource> resource) where TResource : class, IDisposable
    {
        if (_staticCache.TryGetValue(resource.Id, out var managedResource))
        {
            if (managedResource is TResource res1)
                return res1;

            throw new InvalidCastException($"Can not cast resource to {typeof(TResource).Name}.");
        }

        var res2 = resource.Update(_factory);

        _staticCache[resource.Id] = res2;

        return res2;
    }

    public void Dispose()
    {
        ReleaseAll();
        _factory.Dispose();
        GC.SuppressFinalize(this);
    }

    public static IResource<TUser, TResource> Register<TUser, TResource>(Func<D2DResourceFactory, TUser, TResource> updateCallback)
        where TUser : class, IResourceUser
        where TResource : class, IDisposable =>
        new Resource<TUser, TResource>(_lastId++, updateCallback);

    public static IStaticResource<TResource> RegisterStatic<TResource>(Func<D2DResourceFactory, TResource> updateCallback)
        where TResource : class, IDisposable =>
        new StaticResource<TResource>(_lastStaticId++, updateCallback);

    private class Resource<TUser, TResource> : IResource<TUser, TResource>
        where TUser : class, IResourceUser
        where TResource : class, IDisposable
    {
        private readonly Func<D2DResourceFactory, TUser, TResource> _updateCallback;

        public Resource(long id, Func<D2DResourceFactory, TUser, TResource> updateCallback)
        {
            _updateCallback = updateCallback;
            Id = id;
        }

        public long Id { get; }

        public TResource Update(D2DResourceFactory factory, TUser user) =>
            _updateCallback.Invoke(factory, user);
    }

    private class StaticResource<TResource> : IStaticResource<TResource> where TResource : class, IDisposable
    {
        private readonly Func<D2DResourceFactory, TResource> _updateCallback;

        public StaticResource(long id, Func<D2DResourceFactory, TResource> updateCallback)
        {
            _updateCallback = updateCallback;
            Id = id;
        }

        public long Id { get; }

        public TResource Update(D2DResourceFactory factory)
            => _updateCallback.Invoke(factory);
    }
}
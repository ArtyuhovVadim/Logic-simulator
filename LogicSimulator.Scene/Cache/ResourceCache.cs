using LogicSimulator.Scene.DirectX;

namespace LogicSimulator.Scene.Cache;

public class ResourceCache : IDisposable
{
    private readonly Dictionary<Guid, Dictionary<long, IDisposable>> _cache = new();
    private readonly Dictionary<long, IDisposable> _staticCache = new();

    private static long _lastId;

    private static long _lastStaticId;

    private readonly D2DResourceFactory _factory;

    public ResourceCache(D2DResourceFactory factory) => _factory = factory;

    public void Update(IResourceUser user, IResource resource)
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

    public T Get<T>(IResourceUser user, IResource resource) where T : IDisposable
    {
        if (_cache.TryGetValue(user.Id, out var resourceMap))
        {
            if (resourceMap.TryGetValue(resource.Id, out var managedResource))
            {
                if (managedResource is T t)
                    return t;

                throw new InvalidCastException($"Can not cast resource to {typeof(T).Name}.");
            }
        }
        else
        {
            _cache[user.Id] = new Dictionary<long, IDisposable>(1);
        }

        var managedResource1 = resource.Update(_factory, user);

        _cache[user.Id][resource.Id] = managedResource1;

        if (managedResource1 is T t1)
            return t1;

        throw new InvalidCastException($"Can not cast resource to {typeof(T).Name}.");
    }

    public T Get<T>(IStaticResource resource) where T : IDisposable
    {
        if (_staticCache.TryGetValue(resource.Id, out var managedResource))
        {
            if (managedResource is T t)
                return t;

            throw new InvalidCastException($"Can not cast resource to {typeof(T).Name}.");
        }

        var managedResource1 = resource.Update(_factory);

        _staticCache[resource.Id] = managedResource1;

        if (managedResource1 is T t1)
            return t1;

        throw new InvalidCastException($"Can not cast resource to {typeof(T).Name}.");
    }

    public void Dispose()
    {
        ReleaseAll();
        _factory.Dispose();
        GC.SuppressFinalize(this);
    }

    public static IResource Register<TUser>(Func<D2DResourceFactory, TUser, IDisposable> updateCallback)
        where TUser : class, IResourceUser => new Resource<TUser>(_lastId++, updateCallback);

    public static IStaticResource RegisterStatic(Func<D2DResourceFactory, IDisposable> updateCallback)
         => new StaticResource(_lastStaticId++, updateCallback);

    private class Resource<TUser> : IResource where TUser : class, IResourceUser
    {
        private readonly Func<D2DResourceFactory, TUser, IDisposable> _updateCallback;

        public Resource(long id, Func<D2DResourceFactory, TUser, IDisposable> updateCallback)
        {
            _updateCallback = updateCallback;
            Id = id;
        }

        public long Id { get; }

        public IDisposable Update(D2DResourceFactory factory, IResourceUser user) =>
            _updateCallback.Invoke(factory, (TUser)user);
    }

    private class StaticResource : IStaticResource
    {
        private readonly Func<D2DResourceFactory, IDisposable> _updateCallback;

        public StaticResource(long id, Func<D2DResourceFactory, IDisposable> updateCallback)
        {
            _updateCallback = updateCallback;
            Id = id;
        }

        public long Id { get; }

        public IDisposable Update(D2DResourceFactory factory)
            => _updateCallback.Invoke(factory);
    }
}
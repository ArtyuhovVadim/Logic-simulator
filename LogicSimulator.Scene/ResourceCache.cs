using System;
using System.Collections.Generic;
using SharpDX;

namespace LogicSimulator.Scene;

public static class ResourceCache
{
    /*
        uint a = 123;
        uint b = 33;
        
        ulong c = (ulong)a << 32 | b;
        
        uint aa = (uint)(c >> 32);
        uint bb = (uint)(c & 0xffffffffL);
    */

    private static readonly HashSet<ulong> ResourcesToUpdate = new();

    private static readonly Dictionary<ulong, object> Cache = new();

    public static Resource Register(ResourceChangedCallback callback)
    {
        return new Resource(callback);
    }

    public static void RequestUpdateInAllResources()
    {
        foreach (var id in Cache.Keys)
        {
            if (ResourcesToUpdate.Contains(id)) continue;

            ResourcesToUpdate.Add(id);
        }
    }

    public static void InitializeResource(ResourceDependentObject obj, Resource resource, Scene2D scene)
    {
        var id = (ulong)obj.Id << 32 | resource.Id;

        var value = resource.Update(scene, obj);

        Cache[id] = value;
    }

    public static void RequestUpdate(ResourceDependentObject obj, Resource resource)
    {
        var id = (ulong)obj.Id << 32 | resource.Id;

        if (ResourcesToUpdate.Contains(id))
            return;

        ResourcesToUpdate.Add(id);
    }

    public static T GetCached<T>(ResourceDependentObject obj, Resource resource)
    {
        var id = (ulong)obj.Id << 32 | resource.Id;

        return (T)Cache[id];
    }

    public static T GetOrUpdate<T>(ResourceDependentObject obj, Resource resource, Scene2D scene)
    {
        var id = (ulong)obj.Id << 32 | resource.Id;

        var containsInCache = Cache.ContainsKey(id);
        var needUpdate = ResourcesToUpdate.Contains(id);

        switch (containsInCache)
        {
            case true when !needUpdate:
                return (T)Cache[id];
            case true:
                {
                    var cachedResource = Cache[id];

                    if (cachedResource is IDisposable o)
                    {
                        Utilities.Dispose(ref o);
                        Cache[id] = null;
                    }
                    break;
                }
        }

        var value = resource.Update(scene, obj);

        Cache[id] = value;

        ResourcesToUpdate.Remove(id);

        return (T)value;
    }

    public static void ReleaseResources(ResourceDependentObject obj)
    {
        if (!obj.IsInitialized) return;

        foreach (var id in obj.ResourceIds)
        {
            if (Cache[id] is not IDisposable o) continue;

            o.Dispose();
            Cache.Remove(id);
        }
    }
}
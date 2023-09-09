namespace LogicSimulator.Scene.Cache;

public interface ICacheHost
{
    ResourceCache Cache { get; }

    void InitializeCache(ResourceCache cache);
}
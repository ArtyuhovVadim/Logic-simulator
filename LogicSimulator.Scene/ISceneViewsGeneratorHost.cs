namespace LogicSimulator.Scene;

public interface ISceneViewsGeneratorHost
{
    void AddLogicalChild(object child);

    void RemoveLogicalChild(object child);
}
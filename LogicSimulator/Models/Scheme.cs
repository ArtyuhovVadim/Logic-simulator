using LogicSimulator.Scene.SceneObjects.Base;

namespace LogicSimulator.Models;

public class Scheme
{
    public string Name { get; set; }

    public List<BaseSceneObject> Objects { get; set; }
}
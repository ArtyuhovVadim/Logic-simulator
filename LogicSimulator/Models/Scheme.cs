using LogicSimulator.Scene.SceneObjects.Base;

namespace LogicSimulator.Models;

public class Scheme
{
    public const string Extension = ".lss";

    public string Name { get; set; }

    public IEnumerable<BaseSceneObject> Objects { get; set; }
}
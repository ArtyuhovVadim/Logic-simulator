using System.Collections.Generic;
using LogicSimulator.Scene.SceneObjects;

namespace LogicSimulator.Models;

public class Scheme
{
    public string Name { get; set; }

    public List<Rectangle> Objects { get; set; }
}
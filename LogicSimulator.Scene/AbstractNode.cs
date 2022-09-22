﻿using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;

namespace LogicSimulator.Scene;

public abstract class AbstractNode
{
    public static readonly float NodeSize = 4f;

    public abstract Vector2 GetLocation(EditableSceneObject obj);

    public abstract void ApplyMove(EditableSceneObject obj, Vector2 pos);
}
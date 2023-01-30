﻿using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.Scene.Tools.PlacingTools.Base;
using SharpDX;

namespace LogicSimulator.Scene.Tools.PlacingTools;

public class EllipsePlacingTool : BaseObjectPlacingTool<Ellipse>
{
    public EllipsePlacingTool()
    {
        PlacingSteps.Add(new PlacingStep(SetCenter));
        PlacingSteps.Add(new PlacingStep(SetRadiusX, UpdateRadiusX));
        PlacingSteps.Add(new PlacingStep(SetRadiusY, UpdateRadiusY));
    }

    private void SetCenter(Scene2D scene, Vector2 pos)
    {
        PlacingObject.Location = pos;
    }

    private void UpdateRadiusX(Scene2D scene, Vector2 pos)
    {
        PlacingObject.RadiusX = Math.Abs((pos - PlacingObject.Location).X);
    }

    private void SetRadiusX(Scene2D scene, Vector2 pos)
    {
        PlacingObject.RadiusX = Math.Abs((pos - PlacingObject.Location).X);
    }

    private void UpdateRadiusY(Scene2D scene, Vector2 pos)
    {
        PlacingObject.RadiusY = Math.Abs((pos - PlacingObject.Location).Y);
    }

    private void SetRadiusY(Scene2D scene, Vector2 pos)
    {
        PlacingObject.RadiusY = Math.Abs((pos - PlacingObject.Location).Y);
    }
}
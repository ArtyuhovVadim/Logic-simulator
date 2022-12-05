using LogicSimulator.Scene.Tools.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.Scene.Tools.PlacingTools;

public class RectanglePlacingTool : BaseObjectPlacingTool<Rectangle>
{
    public RectanglePlacingTool()
    {
        PlacingSteps.Add(new PlacingStep(SetLocation));
        PlacingSteps.Add(new PlacingStep(SetSize, UpdateSize));
    }

    private void SetLocation(Scene2D scene, Vector2 pos)
    {
        PlacingObject.Location = pos;
    }

    private void UpdateSize(Scene2D scene, Vector2 pos)
    {
        var size = pos - PlacingObject.Location;

        PlacingObject.Width = size.X;
        PlacingObject.Height = size.Y;
    }

    private void SetSize(Scene2D scene, Vector2 pos)
    {
        var size = pos - PlacingObject.Location;

        PlacingObject.Width = size.X;
        PlacingObject.Height = size.Y;
    }
}


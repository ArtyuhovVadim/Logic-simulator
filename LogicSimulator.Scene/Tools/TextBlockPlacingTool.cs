using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.Scene.Tools.Base;
using SharpDX;

namespace LogicSimulator.Scene.Tools;

public class TextBlockPlacingTool : BaseObjectPlacingTool<TextBlock>
{
    public TextBlockPlacingTool()
    {
        PlacingSteps.Add(new PlacingStep(SetLocation));
    }

    private void SetLocation(Scene2D scene, Vector2 pos)
    {
        PlacingObject.Location = pos;
    }
}
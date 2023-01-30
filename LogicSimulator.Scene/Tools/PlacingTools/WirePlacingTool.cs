using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.Scene.SceneObjects.Gates.Base;
using LogicSimulator.Scene.Tools.PlacingTools.Base;
using SharpDX;

namespace LogicSimulator.Scene.Tools.PlacingTools;

public class WirePlacingTool : BaseObjectPlacingTool<WireView>
{
    public WirePlacingTool()
    {
        PlacingSteps.Add(new PlacingStep(SetPoint1));
        PlacingSteps.Add(new PlacingStep(SetPoint2, SetPoint2));
    }

    private Port _firstPort;
    private Port _secondPort;

    private void SetPoint1(Scene2D scene, Vector2 pos)
    {
        PlacingObject.FirstPoint = pos;

        foreach (var gate in scene.Objects.OfType<AbstractGateView>())
        {
            var portsPositions = gate.GetPortsPositions().ToArray();

            for (var i = 0; i < portsPositions.Length; i++)
            {
                var portPos = portsPositions[i];

                if (IsPointInCircle(pos, portPos, 20f))
                {
                    _firstPort = gate.GetPort(i);
                    PlacingObject.FirstPoint = portPos;
                    break;
                }
            }
        }
    }

    private void SetPoint2(Scene2D scene, Vector2 pos)
    {
        PlacingObject.SecondPoint = pos;

        foreach (var gate in scene.Objects.OfType<AbstractGateView>())
        {
            var portsPositions = gate.GetPortsPositions().ToArray();

            for (var i = 0; i < portsPositions.Length; i++)
            {
                var portPos = portsPositions[i];

                if (IsPointInCircle(pos, portPos, 20f))
                {
                    _secondPort = gate.GetPort(i);
                    PlacingObject.SecondPoint = portPos;
                    break;
                }
            }
        }
    }

    protected override void OnEndObjectPlacing()
    {
        if(_firstPort is null || _secondPort is null) return;

        new Wire(_firstPort, _secondPort);
        _firstPort = null;
        _secondPort = null;
    }

    private bool IsPointInCircle(Vector2 pos, Vector2 center, float radius)
    {
        var dx = pos.X - center.X;
        var dy = pos.Y - center.Y;

        return dx * dx + dy * dy < radius * radius;
    }
}
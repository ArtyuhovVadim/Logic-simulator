using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.Utils;
using SharpDX;
using System.Windows.Input;

namespace LogicSimulator.Scene.Tools;

//TODO: Пофиксить
public class TransformTool : BaseTool
{
    private Vector2 _lastMiddleButtonDownPosWithDpi;
    private System.Windows.Point _lastMiddleButtonDownPos;

    private Vector2 _lastRightButtonDownSceneTranslate;
    private Vector2 _lastRightButtonDownPos;

    public float MaxScale { get; set; } = 20f;

    public float MinScale { get; set; } = 0.5f;

    public float MouseScaleStep { get; set; } = 0.01f;

    public float WheelScaleStep { get; set; } = 0.002f;

    public Key WheelScaleButton { get; set; } = Key.LeftCtrl;

    internal override void MouseRightButtonDown(Scene2D scene, Vector2 pos)
    {
        _lastRightButtonDownSceneTranslate = scene.Translation;
        _lastRightButtonDownPos = pos;
    }

    internal override void MouseMiddleButtonDown(Scene2D scene, Vector2 pos)
    {
        _lastMiddleButtonDownPosWithDpi = pos;
        _lastMiddleButtonDownPos = scene.PointToScreen(Mouse.GetPosition(scene));
    }

    internal override void MouseWheel(Scene2D scene, Vector2 pos, int delta)
    {
        if (Mouse.MiddleButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed) return;

        if (!Keyboard.IsKeyDown(WheelScaleButton)) return;

        RelativeScale(scene, pos, WheelScaleStep * delta, MaxScale, MinScale);
    }

    internal override void MouseMove(Scene2D scene, Vector2 pos)
    {
        if (Mouse.MiddleButton == MouseButtonState.Pressed)
        {
            User32.SetCursorPos((int)_lastMiddleButtonDownPos.X, (int)_lastMiddleButtonDownPos.Y);
            RelativeScale(scene, _lastMiddleButtonDownPosWithDpi, -MouseScaleStep * (pos.Y - _lastMiddleButtonDownPosWithDpi.Y), MaxScale, MinScale);
        }
        else if (Mouse.RightButton == MouseButtonState.Pressed)
        {
            scene.Translation = _lastRightButtonDownSceneTranslate + pos - _lastRightButtonDownPos;
        }
    }

    //TODO: Поработать с этой функцией
    private void RelativeScale(Scene2D scene, Vector2 pos, float delta, float max = 20f, float min = 0.5f)
    {
        var p = pos.InvertAndTransform(scene.Transform);

        var newScaleCoefficient = 1 + delta / scene.Scale;
        var newScale = (float)Math.Round(scene.Scale * newScaleCoefficient, 2);

        if (newScale < min || newScale > max) return;

        scene.Translation += p * ((1 - newScaleCoefficient) * scene.Scale);

        scene.Scale = newScale;
    }
}
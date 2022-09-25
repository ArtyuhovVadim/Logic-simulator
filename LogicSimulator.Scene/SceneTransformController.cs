using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using LogicSimulator.Extensions;
using SharpDX;

namespace LogicSimulator.Scene;

public class SceneTransformController
{
    private readonly Scene2D _scene;

    private Vector2 _lastMiddleButtonDownPosWithDpi;
    private System.Windows.Point _lastMiddleButtonDownPos;

    private Vector2 _lastRightButtonDownSceneTranslate;
    private Vector2 _lastRightButtonDownPos;

    public float Max { get; set; } = 20f;

    public float Min { get; set; } = 0.5f;

    public float ScaleStep { get; set; } = 0.01f;

    public SceneTransformController(Scene2D scene)
    {
        _scene = scene;

        scene.MouseMove += OnSceneMouseMove;
        scene.MouseRightButtonDown += OnSceneMouseRightButtonDown;
        scene.MouseDown += OnSceneMouseDown;
    }

    private void OnSceneMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        _lastRightButtonDownSceneTranslate = _scene.Translation;
        _lastRightButtonDownPos = e.GetPosition(_scene).ToVector2().DpiCorrect(_scene.Dpi);
    }

    private void OnSceneMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.MiddleButton == MouseButtonState.Pressed)
        {
            var pos = e.GetPosition(_scene);

            _lastMiddleButtonDownPosWithDpi = pos.ToVector2().DpiCorrect(_scene.Dpi);
            _lastMiddleButtonDownPos = _scene.PointToScreen(pos);
        }
    }

    private void OnSceneMouseMove(object sender, MouseEventArgs e)
    {
        var pos = e.GetPosition(_scene).ToVector2().DpiCorrect(_scene.Dpi);

        if (e.MiddleButton == MouseButtonState.Pressed)
        {
            SetCursorPos((int)_lastMiddleButtonDownPos.X, (int)_lastMiddleButtonDownPos.Y);
            RelativeScale(_lastMiddleButtonDownPosWithDpi, -ScaleStep * (pos.Y - _lastMiddleButtonDownPosWithDpi.Y), Max, Min);
        }
        else if (e.RightButton == MouseButtonState.Pressed)
        {
            _scene.Translation = _lastRightButtonDownSceneTranslate + pos - _lastRightButtonDownPos;
        }
    }

    private void RelativeScale(Vector2 pos, float delta, float max = 20f, float min = 0.5f)
    {
        var p = pos.Transform(_scene.Transform);

        var newScaleCoefficient = 1 + delta / _scene.Scale;
        var newScale = (float)Math.Round(_scene.Scale * newScaleCoefficient, 2);

        if (newScale < min || newScale > max) return;

        _scene.Translation += p * ((1 - newScaleCoefficient) * _scene.Scale);

        _scene.Scale = newScale;
    }

    //TODO: Перенести
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);
}
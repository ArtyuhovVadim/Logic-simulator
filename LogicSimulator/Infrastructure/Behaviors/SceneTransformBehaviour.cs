using System.Windows;
using LogicSimulator.Scene;
using LogicSimulator.Utils;
using Microsoft.Xaml.Behaviors;
using SharpDX;

namespace LogicSimulator.Infrastructure.Behaviors;

public class SceneTransformBehaviour : Behavior<Scene2D>
{
    private Vector2 _lastMiddleButtonDownPosWithDpi;
    private System.Windows.Point _lastMiddleButtonDownPos;

    private Vector2 _lastRightButtonDownSceneTranslate;
    private Vector2 _lastRightButtonDownPos;

    private bool _isMouseRightButtonPressedOnScene;
    private bool _isMouseMiddleButtonPressedOnScene;

    private bool _isTranslationDiffStored;
    private Vector2 _translationDiff;

    #region MaxScale

    public double MaxScale
    {
        get => (double)GetValue(MaxScaleProperty);
        set => SetValue(MaxScaleProperty, value);
    }

    public static readonly DependencyProperty MaxScaleProperty =
        DependencyProperty.Register(nameof(MaxScale), typeof(double), typeof(SceneTransformBehaviour), new PropertyMetadata(20d));

    #endregion

    #region MinScale

    public double MinScale
    {
        get => (double)GetValue(MinScaleProperty);
        set => SetValue(MinScaleProperty, value);
    }

    public static readonly DependencyProperty MinScaleProperty =
        DependencyProperty.Register(nameof(MinScale), typeof(double), typeof(SceneTransformBehaviour), new PropertyMetadata(0.5d));

    #endregion

    #region MouseScaleStep

    public double MouseScaleStep
    {
        get => (double)GetValue(MouseScaleStepProperty);
        set => SetValue(MouseScaleStepProperty, value);
    }

    public static readonly DependencyProperty MouseScaleStepProperty =
        DependencyProperty.Register(nameof(MouseScaleStep), typeof(double), typeof(SceneTransformBehaviour), new PropertyMetadata(0.01d));

    #endregion

    #region WheelScaleStep

    public double WheelScaleStep
    {
        get => (double)GetValue(WheelScaleStepProperty);
        set => SetValue(WheelScaleStepProperty, value);
    }

    public static readonly DependencyProperty WheelScaleStepProperty =
        DependencyProperty.Register(nameof(WheelScaleStep), typeof(double), typeof(SceneTransformBehaviour), new PropertyMetadata(0.002d));

    #endregion

    #region WheelScaleButton

    public Key WheelScaleButton
    {
        get => (Key)GetValue(WheelScaleButtonProperty);
        set => SetValue(WheelScaleButtonProperty, value);
    }

    public static readonly DependencyProperty WheelScaleButtonProperty =
        DependencyProperty.Register(nameof(WheelScaleButton), typeof(Key), typeof(SceneTransformBehaviour), new PropertyMetadata(Key.LeftCtrl));

    #endregion

    #region TranslationThreshold

    public float TranslationThreshold
    {
        get => (float)GetValue(TranslationThresholdProperty);
        set => SetValue(TranslationThresholdProperty, value);
    }

    public static readonly DependencyProperty TranslationThresholdProperty =
        DependencyProperty.Register(nameof(TranslationThreshold), typeof(float), typeof(SceneTransformBehaviour), new PropertyMetadata(3f));

    #endregion

    protected override void OnAttached()
    {
        AssociatedObject.MouseDown += OnSceneMouseDown;
        AssociatedObject.MouseUp += OnSceneMouseUp;
        AssociatedObject.MouseWheel += OnSceneMouseWheel;
        AssociatedObject.MouseMove += OnSceneMouseMove;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.MouseDown -= OnSceneMouseDown;
        AssociatedObject.MouseUp -= OnSceneMouseUp;
        AssociatedObject.MouseWheel -= OnSceneMouseWheel;
        AssociatedObject.MouseMove -= OnSceneMouseMove;
    }

    private void OnSceneMouseDown(object sender, MouseButtonEventArgs e)
    {
        var pos = e.GetPosition(AssociatedObject).ToVector2().DpiCorrect(AssociatedObject.Dpi);

        switch (e.ChangedButton)
        {
            case MouseButton.Right:
                _lastRightButtonDownSceneTranslate = AssociatedObject.Translation;
                _lastRightButtonDownPos = pos;
                _isMouseRightButtonPressedOnScene = true;
                break;
            case MouseButton.Middle:
                _lastMiddleButtonDownPosWithDpi = pos;
                _lastMiddleButtonDownPos = AssociatedObject.PointToScreen(e.GetPosition(AssociatedObject));
                _isMouseMiddleButtonPressedOnScene = true;
                break;
        }

        Mouse.Capture(AssociatedObject);
        Keyboard.Focus(AssociatedObject);
    }

    private void OnSceneMouseUp(object sender, MouseButtonEventArgs e)
    {
        switch (e.ChangedButton)
        {
            case MouseButton.Right:
                _isMouseRightButtonPressedOnScene = false;
                _isTranslationDiffStored = false;
                break;
            case MouseButton.Middle:
                _isMouseMiddleButtonPressedOnScene = false;
                break;
        }

        Mouse.Capture(null);
    }

    private void OnSceneMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var pos = e.GetPosition(AssociatedObject).ToVector2().DpiCorrect(AssociatedObject.Dpi);

        if (e.MiddleButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed) return;

        if (!Keyboard.IsKeyDown(WheelScaleButton)) return;

        RelativeScale(AssociatedObject, pos, WheelScaleStep * e.Delta, MaxScale, MinScale);
    }

    private void OnSceneMouseMove(object sender, MouseEventArgs e)
    {
        var pos = e.GetPosition(AssociatedObject).ToVector2().DpiCorrect(AssociatedObject.Dpi);

        if (e.MiddleButton == MouseButtonState.Pressed && _isMouseMiddleButtonPressedOnScene)
        {
            User32.SetCursorPos((int)_lastMiddleButtonDownPos.X, (int)_lastMiddleButtonDownPos.Y);
            RelativeScale(AssociatedObject, _lastMiddleButtonDownPosWithDpi, -MouseScaleStep * (pos.Y - _lastMiddleButtonDownPosWithDpi.Y), MaxScale, MinScale);
        }
        else if (e.RightButton == MouseButtonState.Pressed && _isMouseRightButtonPressedOnScene)
        {
            var diff = pos - _lastRightButtonDownPos;

            if (_isTranslationDiffStored || diff.Length() >= TranslationThreshold)
            {
                if (!_isTranslationDiffStored)
                {
                    _translationDiff = diff; 
                    _isTranslationDiffStored = true;
                }

                AssociatedObject.Translation = _lastRightButtonDownSceneTranslate + pos - _lastRightButtonDownPos - _translationDiff;
            }
        }
    }

    private static void RelativeScale(Scene2D scene, Vector2 pos, double delta, double max, double min)
    {
        var p = pos.InvertAndTransform(scene.Transform);

        var newScaleCoefficient = 1 + delta / scene.Scale;
        var newScale = (float)Math.Round(scene.Scale * newScaleCoefficient, 2);

        if (newScale < min || newScale > max) return;

        scene.Translation += p * (float)((1 - newScaleCoefficient) * scene.Scale);

        scene.Scale = newScale;
    }
}
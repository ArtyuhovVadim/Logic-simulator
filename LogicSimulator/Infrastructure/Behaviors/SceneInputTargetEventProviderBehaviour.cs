using System.Windows;
using LogicSimulator.Models.Input;
using LogicSimulator.Scene;
using Microsoft.Xaml.Behaviors;
using SharpDX;

namespace LogicSimulator.Infrastructure.Behaviors;

public class SceneInputTargetEventProviderBehaviour : Behavior<Scene2D>
{
    private bool _isMouseLeftButtonPressedOnScene;
    private bool _isMouseRightButtonPressedOnScene;
    private bool _isMouseMiddleButtonPressedOnScene;

    private Vector2 _lastMouseLeftButtonPosition;
    private Vector2 _lastMouseRightButtonPosition;
    private Vector2 _lastMouseMiddleButtonPosition;

    #region InputTarget

    public IInputTarget? InputTarget
    {
        get => (IInputTarget?)GetValue(InputTargetProperty);
        set => SetValue(InputTargetProperty, value);
    }

    public static readonly DependencyProperty InputTargetProperty =
        DependencyProperty.Register(nameof(InputTarget), typeof(IInputTarget), typeof(SceneInputTargetEventProviderBehaviour), new PropertyMetadata(default(IInputTarget?)));

    #endregion

    protected override void OnAttached()
    {
        AssociatedObject.PreviewMouseDown += OnSceneMouseDown;
        AssociatedObject.PreviewMouseMove += OnSceneMouseMove;
        AssociatedObject.PreviewMouseUp += OnSceneMouseUp;
        AssociatedObject.PreviewMouseWheel += OnSceneMouseWheel;
        AssociatedObject.PreviewKeyDown += OnSceneKeyDown;
        AssociatedObject.PreviewKeyUp += OnSceneKeyUp;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.PreviewMouseDown -= OnSceneMouseDown;
        AssociatedObject.PreviewMouseMove -= OnSceneMouseMove;
        AssociatedObject.PreviewMouseUp -= OnSceneMouseUp;
        AssociatedObject.PreviewMouseWheel -= OnSceneMouseWheel;
        AssociatedObject.PreviewKeyDown -= OnSceneKeyDown;
        AssociatedObject.PreviewKeyUp -= OnSceneKeyUp;
    }

    private void OnSceneMouseDown(object sender, MouseButtonEventArgs e)
    {
        var pos = GetMousePos(e);
        var args = new InputArgs(pos);

        switch (e.ChangedButton)
        {
            case MouseButton.Left:
                _isMouseLeftButtonPressedOnScene = true;
                _lastMouseLeftButtonPosition = pos;
                InputTarget?.MouseLeftButtonDown(args);
                break;
            case MouseButton.Right:
                _isMouseRightButtonPressedOnScene = true;
                _lastMouseRightButtonPosition = pos;
                InputTarget?.MouseRightButtonDown(args);
                break;
            case MouseButton.Middle:
                _isMouseMiddleButtonPressedOnScene = true;
                _lastMouseMiddleButtonPosition = pos;
                InputTarget?.MouseMiddleButtonDown(args);
                break;
        }

        Mouse.Capture(AssociatedObject);
        Keyboard.Focus(AssociatedObject);
    }

    private void OnSceneMouseMove(object sender, MouseEventArgs e)
    {
        var pos = GetMousePos(e);

        if (e.LeftButton == MouseButtonState.Pressed && _isMouseLeftButtonPressedOnScene)
            InputTarget?.MouseLeftButtonDragged(new DragInputArgs(_lastMouseLeftButtonPosition, pos, pos - _lastMouseLeftButtonPosition));

        if (e.RightButton == MouseButtonState.Pressed && _isMouseRightButtonPressedOnScene)
            InputTarget?.MouseRightButtonDragged(new DragInputArgs(_lastMouseRightButtonPosition, pos, pos - _lastMouseRightButtonPosition));

        if (e.MiddleButton == MouseButtonState.Pressed && _isMouseMiddleButtonPressedOnScene)
            InputTarget?.MouseMiddleButtonDragged(new DragInputArgs(_lastMouseMiddleButtonPosition, pos, pos - _lastMouseMiddleButtonPosition));

        InputTarget?.MouseMove(new InputArgs(pos));
    }

    private void OnSceneMouseUp(object sender, MouseButtonEventArgs e)
    {
        var pos = GetMousePos(e);
        var args = new InputArgs(pos);

        switch (e.ChangedButton)
        {
            case MouseButton.Left:
                if (_isMouseLeftButtonPressedOnScene) InputTarget?.MouseLeftButtonUp(args);
                _isMouseLeftButtonPressedOnScene = false;
                break;
            case MouseButton.Right:
                if (_isMouseRightButtonPressedOnScene) InputTarget?.MouseRightButtonUp(args);
                _isMouseRightButtonPressedOnScene = false;
                break;
            case MouseButton.Middle:
                if (_isMouseMiddleButtonPressedOnScene) InputTarget?.MouseMiddleButtonUp(args);
                _isMouseMiddleButtonPressedOnScene = false;
                break;
        }

        Mouse.Capture(null);
    }

    private void OnSceneMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var pos = GetMousePos(e);
        var args = new WheelInputArgs(pos, e.Delta);

        InputTarget?.MouseWheel(args);
    }

    private void OnSceneKeyDown(object sender, KeyEventArgs e)
    {
        var pos = AssociatedObject.PointFromControlToSceneSpace(Mouse.GetPosition(AssociatedObject));
        var args = new KeyInputArgs(e.Key, Keyboard.Modifiers, pos);
        InputTarget?.KeyDown(args);
    }

    private void OnSceneKeyUp(object sender, KeyEventArgs e)
    {
        var pos = AssociatedObject.PointFromControlToSceneSpace(Mouse.GetPosition(AssociatedObject));
        var args = new KeyInputArgs(e.Key, Keyboard.Modifiers, pos);
        InputTarget?.KeyUp(args);
    }

    private Vector2 GetMousePos(MouseEventArgs e) =>
        AssociatedObject.PointFromControlToSceneSpace(e.GetPosition(AssociatedObject));
}
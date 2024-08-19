using System.Windows;
using LogicSimulator.Models.Input;
using LogicSimulator.Scene;
using Microsoft.Xaml.Behaviors;
using SharpDX;

namespace LogicSimulator.Infrastructure.Behaviors;

public class SceneToolEventProviderBehaviour : Behavior<Scene2D>
{
    private bool _isMouseLeftButtonPressedOnScene;
    private bool _isMouseRightButtonPressedOnScene;
    private bool _isMouseMiddleButtonPressedOnScene;

    private Vector2 _lastMouseLeftButtonPosition;
    private Vector2 _lastMouseRightButtonPosition;
    private Vector2 _lastMouseMiddleButtonPosition;

    #region CurrentTool

    public ITool? CurrentTool
    {
        get => (ITool?)GetValue(CurrentToolProperty);
        set => SetValue(CurrentToolProperty, value);
    }

    public static readonly DependencyProperty CurrentToolProperty =
        DependencyProperty.Register(nameof(CurrentTool), typeof(ITool), typeof(SceneToolEventProviderBehaviour), new PropertyMetadata(default(ITool?)));

    #endregion

    protected override void OnAttached()
    {
        AssociatedObject.MouseDown += OnSceneMouseDown;
        AssociatedObject.MouseMove += OnSceneMouseMove;
        AssociatedObject.MouseUp += OnSceneMouseUp;
        AssociatedObject.MouseWheel += OnSceneMouseWheel;
        AssociatedObject.KeyDown += OnSceneKeyDown;
        AssociatedObject.KeyUp += OnSceneKeyUp;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.MouseDown -= OnSceneMouseDown;
        AssociatedObject.MouseMove -= OnSceneMouseMove;
        AssociatedObject.MouseUp -= OnSceneMouseUp;
        AssociatedObject.MouseWheel -= OnSceneMouseWheel;
        AssociatedObject.KeyDown -= OnSceneKeyDown;
        AssociatedObject.KeyUp -= OnSceneKeyUp;
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
                CurrentTool?.MouseLeftButtonDown(args);
                break;
            case MouseButton.Right:
                _isMouseRightButtonPressedOnScene = true;
                _lastMouseRightButtonPosition = pos;
                CurrentTool?.MouseRightButtonDown(args);
                break;
            case MouseButton.Middle:
                _isMouseMiddleButtonPressedOnScene = true;
                _lastMouseMiddleButtonPosition = pos;
                CurrentTool?.MouseMiddleButtonDown(args);
                break;
        }

        Mouse.Capture(AssociatedObject);
        Keyboard.Focus(AssociatedObject);
    }

    private void OnSceneMouseMove(object sender, MouseEventArgs e)
    {
        var pos = GetMousePos(e);

        if (e.LeftButton == MouseButtonState.Pressed && _isMouseLeftButtonPressedOnScene)
            CurrentTool?.MouseLeftButtonDragged(new DragInputArgs(_lastMouseLeftButtonPosition, pos, pos - _lastMouseLeftButtonPosition));

        if (e.RightButton == MouseButtonState.Pressed && _isMouseRightButtonPressedOnScene)
            CurrentTool?.MouseRightButtonDragged(new DragInputArgs(_lastMouseRightButtonPosition, pos, pos - _lastMouseRightButtonPosition));

        if (e.MiddleButton == MouseButtonState.Pressed && _isMouseMiddleButtonPressedOnScene)
            CurrentTool?.MouseMiddleButtonDragged(new DragInputArgs(_lastMouseMiddleButtonPosition, pos, pos - _lastMouseMiddleButtonPosition));

        CurrentTool?.MouseMove(new InputArgs(pos));
    }

    private void OnSceneMouseUp(object sender, MouseButtonEventArgs e)
    {
        var pos = GetMousePos(e);
        var args = new InputArgs(pos);

        switch (e.ChangedButton)
        {
            case MouseButton.Left:
                if (_isMouseLeftButtonPressedOnScene) CurrentTool?.MouseLeftButtonUp(args);
                _isMouseLeftButtonPressedOnScene = false;
                break;
            case MouseButton.Right:
                if (_isMouseRightButtonPressedOnScene) CurrentTool?.MouseRightButtonUp(args);
                _isMouseRightButtonPressedOnScene = false;
                break;
            case MouseButton.Middle:
                if (_isMouseMiddleButtonPressedOnScene) CurrentTool?.MouseMiddleButtonUp(args);
                _isMouseMiddleButtonPressedOnScene = false;
                break;
        }

        Mouse.Capture(null);
    }

    private void OnSceneMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var pos = GetMousePos(e);
        var args = new WheelInputArgs(pos, e.Delta);

        CurrentTool?.MouseWheel(args);
    }

    private void OnSceneKeyDown(object sender, KeyEventArgs e)
    {
        var pos = AssociatedObject.PointFromControlToSceneSpace(Mouse.GetPosition(AssociatedObject));
        var args = new KeyInputArgs(e.Key, Keyboard.Modifiers, pos);
        CurrentTool?.KeyDown(args);
    }

    private void OnSceneKeyUp(object sender, KeyEventArgs e)
    {
        var pos = AssociatedObject.PointFromControlToSceneSpace(Mouse.GetPosition(AssociatedObject));
        var args = new KeyInputArgs(e.Key, Keyboard.Modifiers, pos);
        CurrentTool?.KeyUp(args);
    }

    private Vector2 GetMousePos(MouseEventArgs e) =>
        AssociatedObject.PointFromControlToSceneSpace(e.GetPosition(AssociatedObject));
}
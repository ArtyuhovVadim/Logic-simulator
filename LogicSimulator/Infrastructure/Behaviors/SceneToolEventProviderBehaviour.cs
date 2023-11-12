using System.Windows;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;
using Microsoft.Xaml.Behaviors;
using SharpDX;

namespace LogicSimulator.Infrastructure.Behaviors;

public class SceneToolEventProviderBehaviour : Behavior<Scene2D>
{
    private bool _isMouseLeftButtonPressedOnScene;
    private bool _isMouseRightButtonPressedOnScene;
    private bool _isMouseMiddleButtonPressedOnScene;

    #region CurrentTool

    public BaseTool CurrentTool
    {
        get => (BaseTool)GetValue(CurrentToolProperty);
        set => SetValue(CurrentToolProperty, value);
    }

    public static readonly DependencyProperty CurrentToolProperty =
        DependencyProperty.Register(nameof(CurrentTool), typeof(BaseTool), typeof(SceneToolEventProviderBehaviour), new PropertyMetadata(default(BaseTool)));

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

        switch (e.ChangedButton)
        {
            case MouseButton.Left:
                _isMouseLeftButtonPressedOnScene = true;
                CurrentTool?.MouseLeftButtonDown(AssociatedObject, pos);
                break;
            case MouseButton.Right:
                _isMouseRightButtonPressedOnScene = true;
                CurrentTool?.MouseRightButtonDown(AssociatedObject, pos);
                break;
            case MouseButton.Middle:
                _isMouseMiddleButtonPressedOnScene = true;
                CurrentTool?.MouseMiddleButtonDown(AssociatedObject, pos);
                break;
        }

        Mouse.Capture(AssociatedObject);
        Keyboard.Focus(AssociatedObject);
    }

    private void OnSceneMouseMove(object sender, MouseEventArgs e)
    {
        var pos = GetMousePos(e);

        if (e.LeftButton == MouseButtonState.Pressed && _isMouseLeftButtonPressedOnScene)
            CurrentTool?.MouseLeftButtonDragged(AssociatedObject, pos);

        if (e.RightButton == MouseButtonState.Pressed && _isMouseRightButtonPressedOnScene)
            CurrentTool?.MouseRightButtonDragged(AssociatedObject, pos);

        if (e.MiddleButton == MouseButtonState.Pressed && _isMouseMiddleButtonPressedOnScene)
            CurrentTool?.MouseMiddleButtonDragged(AssociatedObject, pos);

        CurrentTool?.MouseMove(AssociatedObject, pos);
    }

    private void OnSceneMouseUp(object sender, MouseButtonEventArgs e)
    {
        var pos = GetMousePos(e);

        switch (e.ChangedButton)
        {
            case MouseButton.Left:
                if (_isMouseLeftButtonPressedOnScene) CurrentTool?.MouseLeftButtonUp(AssociatedObject, pos);
                _isMouseLeftButtonPressedOnScene = false;
                break;
            case MouseButton.Right:
                if (_isMouseRightButtonPressedOnScene) CurrentTool?.MouseRightButtonUp(AssociatedObject, pos);
                _isMouseRightButtonPressedOnScene = false;
                break;
            case MouseButton.Middle:
                if (_isMouseMiddleButtonPressedOnScene) CurrentTool?.MouseMiddleButtonUp(AssociatedObject, pos);
                _isMouseMiddleButtonPressedOnScene = false;
                break;
        }

        Mouse.Capture(null);
    }

    private void OnSceneMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var pos = GetMousePos(e);

        CurrentTool?.MouseWheel(AssociatedObject, pos, e.Delta);
    }

    private void OnSceneKeyDown(object sender, KeyEventArgs e)
    {
        var pos = AssociatedObject.PointFromControlToSceneSpace(Mouse.GetPosition(AssociatedObject));

        CurrentTool?.KeyDown(AssociatedObject, e, pos);
    }

    private void OnSceneKeyUp(object sender, KeyEventArgs e)
    {
        var pos = AssociatedObject.PointFromControlToSceneSpace(Mouse.GetPosition(AssociatedObject));

        CurrentTool?.KeyUp(AssociatedObject, e, pos);
    }

    private Vector2 GetMousePos(MouseEventArgs e) =>
        AssociatedObject.PointFromControlToSceneSpace(e.GetPosition(AssociatedObject));
}
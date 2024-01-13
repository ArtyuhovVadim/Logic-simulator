using System.Windows;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;
using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public class PlacingTool : BaseTool
{
    private bool _mouseRightButtonDragged;

    public PlacingTool()
    {
        MouseRightButtonDragThreshold = 5;
    }

    #region MouseLeftButtonDownCommand

    public ICommand? MouseLeftButtonDownCommand
    {
        get => (ICommand?)GetValue(MouseLeftButtonDownCommandProperty);
        set => SetValue(MouseLeftButtonDownCommandProperty, value);
    }

    public static readonly DependencyProperty MouseLeftButtonDownCommandProperty =
        DependencyProperty.Register(nameof(MouseLeftButtonDownCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand?)));

    #endregion

    #region MouseRightButtonUpCommand

    public ICommand? MouseRightButtonUpCommand
    {
        get => (ICommand?)GetValue(MouseRightButtonUpCommandProperty);
        set => SetValue(MouseRightButtonUpCommandProperty, value);
    }

    public static readonly DependencyProperty MouseRightButtonUpCommandProperty =
        DependencyProperty.Register(nameof(MouseRightButtonUpCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand?)));

    #endregion

    #region MouseMoveCommand

    public ICommand? MouseMoveCommand
    {
        get => (ICommand?)GetValue(MouseMoveCommandProperty);
        set => SetValue(MouseMoveCommandProperty, value);
    }

    public static readonly DependencyProperty MouseMoveCommandProperty =
        DependencyProperty.Register(nameof(MouseMoveCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand?)));

    #endregion

    #region CancelCommand

    public ICommand? CancelCommand
    {
        get => (ICommand?)GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public static readonly DependencyProperty CancelCommandProperty =
        DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand?)));

    #endregion

    protected override void OnKeyDown(Scene2D scene, KeyEventArgs args, Vector2 pos)
    {
        if (args.Key != CancelKey) return;

        if (CancelCommand?.CanExecute(pos) is not null)
        {
            CancelCommand.Execute(pos);
        }
    }

    protected override void OnMouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        if (MouseLeftButtonDownCommand?.CanExecute(pos) is not null)
        {
            MouseLeftButtonDownCommand.Execute(pos);
        }
    }

    protected override void OnMouseMove(Scene2D scene, Vector2 pos)
    {
        if (MouseMoveCommand?.CanExecute(pos) is not null)
        {
            MouseMoveCommand.Execute(pos);
        }
    }

    protected override void OnMouseRightButtonDragged(Scene2D scene, Vector2 pos) => _mouseRightButtonDragged = true;

    protected override void OnMouseRightButtonUp(Scene2D scene, Vector2 pos)
    {
        if (_mouseRightButtonDragged)
        {
            _mouseRightButtonDragged = false;
            return;
        }

        if (MouseRightButtonUpCommand?.CanExecute(pos) is not null)
        {
            MouseRightButtonUpCommand.Execute(pos);
        }
    }

    protected override Freezable CreateInstanceCore() => new PlacingTool();
}
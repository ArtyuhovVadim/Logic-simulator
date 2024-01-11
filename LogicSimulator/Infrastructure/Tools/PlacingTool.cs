using System.Windows;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;
using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public class PlacingTool : BaseTool
{
    private bool _mouseRightButtonDragged;

    #region ActionCommand

    public ICommand? ActionCommand
    {
        get => (ICommand?)GetValue(ActionCommandProperty);
        set => SetValue(ActionCommandProperty, value);
    }

    public static readonly DependencyProperty ActionCommandProperty =
        DependencyProperty.Register(nameof(ActionCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand?)));

    #endregion

    #region RejectCommand

    public ICommand? RejectCommand
    {
        get => (ICommand?)GetValue(RejectCommandProperty);
        set => SetValue(RejectCommandProperty, value);
    }

    public static readonly DependencyProperty RejectCommandProperty =
        DependencyProperty.Register(nameof(RejectCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand?)));

    #endregion

    #region UpdateCommand

    public ICommand? UpdateCommand
    {
        get => (ICommand?)GetValue(UpdateCommandProperty);
        set => SetValue(UpdateCommandProperty, value);
    }

    public static readonly DependencyProperty UpdateCommandProperty =
        DependencyProperty.Register(nameof(UpdateCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand?)));

    #endregion

    protected override void OnKeyDown(Scene2D scene, KeyEventArgs args, Vector2 pos)
    {
        if (args.Key != CancelKey) return;
        OnCancel();
    }

    protected override void OnMouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        if (ActionCommand?.CanExecute(pos) is not null)
        {
            ActionCommand.Execute(pos);
        }
    }

    protected override void OnMouseMove(Scene2D scene, Vector2 pos)
    {
        if (UpdateCommand?.CanExecute(pos) is not null)
        {
            UpdateCommand.Execute(pos);
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

        OnCancel();
    }

    private void OnCancel()
    {
        if (RejectCommand?.CanExecute(null) is not null)
        {
            RejectCommand.Execute(null);
        }
    }

    protected override Freezable CreateInstanceCore() => new PlacingTool();
}
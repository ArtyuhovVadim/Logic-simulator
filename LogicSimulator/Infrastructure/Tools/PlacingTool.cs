using System.Windows;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;
using SharpDX;

namespace LogicSimulator.Infrastructure.Tools;

public class PlacingTool : BaseTool
{
    private int _currentStep = -1;
    private bool _mouseRightButtonDragged;

    private bool IsStarted => _currentStep != -1;

    private bool InProgress => _currentStep >= 0 && _currentStep < StepsCount - 1;

    private bool IsLastStep => _currentStep == StepsCount - 1;

    #region StepsCount

    public int StepsCount
    {
        get => (int)GetValue(StepsCountProperty);
        set => SetValue(StepsCountProperty, value);
    }

    public static readonly DependencyProperty StepsCountProperty =
        DependencyProperty.Register(nameof(StepsCount), typeof(int), typeof(PlacingTool), new PropertyMetadata(default(int)));

    #endregion

    #region StartPlacingCommand

    public ICommand? StartPlacingCommand
    {
        get => (ICommand?)GetValue(StartPlacingCommandProperty);
        set => SetValue(StartPlacingCommandProperty, value);
    }

    public static readonly DependencyProperty StartPlacingCommandProperty =
        DependencyProperty.Register(nameof(StartPlacingCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand)));

    #endregion

    #region NextPlacingStepCommand

    public ICommand? NextPlacingStepCommand
    {
        get => (ICommand?)GetValue(NextPlacingStepCommandProperty);
        set => SetValue(NextPlacingStepCommandProperty, value);
    }

    public static readonly DependencyProperty NextPlacingStepCommandProperty =
        DependencyProperty.Register(nameof(NextPlacingStepCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand)));

    #endregion

    #region UpdatePlacingCommand

    public ICommand? UpdatePlacingCommand
    {
        get => (ICommand?)GetValue(UpdatePlacingCommandProperty);
        set => SetValue(UpdatePlacingCommandProperty, value);
    }

    public static readonly DependencyProperty UpdatePlacingCommandProperty =
        DependencyProperty.Register(nameof(UpdatePlacingCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand)));

    #endregion

    #region EndPlacingCommand

    public ICommand? EndPlacingCommand
    {
        get => (ICommand?)GetValue(EndPlacingCommandProperty);
        set => SetValue(EndPlacingCommandProperty, value);
    }

    public static readonly DependencyProperty EndPlacingCommandProperty =
        DependencyProperty.Register(nameof(EndPlacingCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand)));

    #endregion

    #region CancelPlacingCommand

    public ICommand? CancelPlacingCommand
    {
        get => (ICommand?)GetValue(CancelPlacingCommandProperty);
        set => SetValue(CancelPlacingCommandProperty, value);
    }

    public static readonly DependencyProperty CancelPlacingCommandProperty =
        DependencyProperty.Register(nameof(CancelPlacingCommand), typeof(ICommand), typeof(PlacingTool), new PropertyMetadata(default(ICommand)));

    #endregion

    protected override void OnKeyDown(Scene2D scene, KeyEventArgs args, Vector2 pos)
    {
        if (args.Key != CancelKey) return;
        OnCancel();
    }

    protected override void OnMouseLeftButtonDown(Scene2D scene, Vector2 pos)
    {
        if (!IsStarted && StartPlacingCommand?.CanExecute(pos) is not null)
        {
            StartPlacingCommand.Execute(pos);
            _currentStep++;
        }
        else if (InProgress && NextPlacingStepCommand?.CanExecute(pos) is not null)
        {
            NextPlacingStepCommand.Execute(pos);
            _currentStep++;
        }
        else if (IsLastStep && EndPlacingCommand?.CanExecute(null) is not null)
        {
            EndPlacingCommand.Execute(null);
            _currentStep = -1;
        }
    }

    protected override void OnMouseMove(Scene2D scene, Vector2 pos)
    {
        if (IsStarted && UpdatePlacingCommand?.CanExecute(pos) is not null)
        {
            UpdatePlacingCommand.Execute(pos);
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
        if (!IsStarted)
        {
            if (EndPlacingCommand?.CanExecute(null) is not null)
            {
                EndPlacingCommand.Execute(null);
            }

            ToolsController.SwitchToDefaultTool();
        }
        else if (CancelPlacingCommand?.CanExecute(null) is not null)
        {
            CancelPlacingCommand.Execute(null);
            _currentStep = -1;
        }
    }

    protected override Freezable CreateInstanceCore() => new PlacingTool();
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Scene.Tools.Base;
using SharpDX;

namespace LogicSimulator.Scene;

public class ToolsController
{
    public event Action SelectedObjectsChanged;

    public BaseTool DefaultTool { get; set; }

    public BaseTool CurrentTool { get; private set; }

    public IEnumerable<BaseTool> Tools { get; set; } = Enumerable.Empty<BaseTool>();

    public IEnumerable<BaseTool> AlwaysUpdatingTools { get; set; } = Enumerable.Empty<BaseTool>();

    public ToolsController(BaseTool defaultTool)
    {
        DefaultTool = defaultTool;

        SwitchToDefaultTool();
    }

    public void SwitchToDefaultTool()
    {
        if (CurrentTool is null)
        {
            DefaultTool.Activate(this);
            CurrentTool = DefaultTool;
            return;
        }

        CurrentTool.Deactivate(this);
        CurrentTool = DefaultTool;
        CurrentTool.Activate(this);
    }

    public void SwitchTool<T>(Action<T> actionToNextToolAfterActivating = null) where T : BaseTool
    {
        var tool = Tools.FirstOrDefault(x => x.GetType() == typeof(T));

        if (tool is null) return;

        if (CurrentTool is null)
        {
            tool.Activate(this);
            CurrentTool = tool;
            return;
        }

        if (!CurrentTool.CanSwitch) return;

        CurrentTool.Deactivate(this);
        CurrentTool = tool;
        CurrentTool.Activate(this);

        actionToNextToolAfterActivating?.Invoke((T)CurrentTool);
    }

    internal void OnSelectedObjectsChanged()
    {
        SelectedObjectsChanged?.Invoke();
    }

    internal void KeyDown(Scene2D scene, KeyEventArgs args) => ProvideEvents(tool => tool.KeyDown(scene, args));

    internal void KeyUp(Scene2D scene, KeyEventArgs args) => ProvideEvents(tool => tool.KeyUp(scene, args));

    internal void MouseMove(Scene2D scene, Vector2 pos) => ProvideEvents(tool => tool.MouseMove(scene, pos));

    internal void MouseLeftButtonDown(Scene2D scene, Vector2 pos) => ProvideEvents(tool => tool.MouseLeftButtonDown(scene, pos));

    internal void MouseLeftButtonDragged(Scene2D scene, Vector2 pos) => ProvideEvents(tool => tool.MouseLeftButtonDragged(scene, pos));

    internal void MouseLeftButtonUp(Scene2D scene, Vector2 pos) => ProvideEvents(tool => tool.MouseLeftButtonUp(scene, pos));

    internal void MouseRightButtonDown(Scene2D scene, Vector2 pos) => ProvideEvents(tool => tool.MouseRightButtonDown(scene, pos));

    internal void MouseRightButtonDragged(Scene2D scene, Vector2 pos) => ProvideEvents(tool => tool.MouseRightButtonDragged(scene, pos));

    internal void MouseRightButtonUp(Scene2D scene, Vector2 pos) => ProvideEvents(tool => tool.MouseRightButtonUp(scene, pos));

    internal void MouseMiddleButtonDown(Scene2D scene, Vector2 pos) => ProvideEvents(tool => tool.MouseMiddleButtonDown(scene, pos));

    internal void MouseMiddleButtonDragged(Scene2D scene, Vector2 pos) => ProvideEvents(tool => tool.MouseMiddleButtonDragged(scene, pos));

    internal void MouseMiddleButtonUp(Scene2D scene, Vector2 pos) => ProvideEvents(tool => tool.MouseMiddleButtonUp(scene, pos));

    private void ProvideEvents(Action<BaseTool> action)
    {
        foreach (var alwaysUpdatingTool in AlwaysUpdatingTools)
        {
            action.Invoke(alwaysUpdatingTool);
        }

        if (CurrentTool is null) return;

        action.Invoke(CurrentTool);
    }
}
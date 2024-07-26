using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Infrastructure.Tools;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels.Tools;

public enum ToolGroup
{
    Generic,
    BaseGeometryPlacing,
    GatesPlacing,
    WirePlacing
}

public abstract class BaseToolViewModel : BindableBase, ITool
{
    public string Name { get; set; } = string.Empty;

    public Key CanselKey { get; set; } = Key.Escape;

    public bool ActivatedFromOtherTool { get; private set; }

    public ToolGroup Group { get; set; } = ToolGroup.Generic;

    protected IToolSwitcherService ToolSwitcher { get; private set; } = null!;

    public void Activate(IToolSwitcherService toolSwitcher, bool activatedFromOtherTool)
    {
        ToolSwitcher = toolSwitcher;
        ActivatedFromOtherTool = activatedFromOtherTool;
        OnActivated();
    }

    public void Deactivate()
    {
        ActivatedFromOtherTool = false;
        OnDeactivated();
    }

    public void MouseLeftButtonDown(InputArgs args) => OnMouseLeftButtonDown(args);

    public void MouseLeftButtonDragged(DragInputArgs args) => OnMouseLeftButtonDragged(args);

    public void MouseLeftButtonUp(InputArgs args) => OnMouseLeftButtonUp(args);

    public void MouseRightButtonDown(InputArgs args) => OnMouseRightButtonDown(args);

    public void MouseRightButtonDragged(DragInputArgs args) => OnMouseRightButtonDragged(args);

    public void MouseRightButtonUp(InputArgs args) => OnMouseRightButtonUp(args);

    public void MouseMiddleButtonDown(InputArgs args) => OnMouseMiddleButtonDown(args);

    public void MouseMiddleButtonDragged(DragInputArgs args) => OnMouseMiddleButtonDragged(args);

    public void MouseMiddleButtonUp(InputArgs args) => OnMouseMiddleButtonUp(args);

    public void MouseMove(InputArgs args) => OnMouseMove(args);

    public void MouseWheel(WheelInputArgs args) => OnMouseWheel(args);

    public void KeyDown(KeyInputArgs args) => OnKeyDown(args);

    public void KeyUp(KeyInputArgs args) => OnKeyUp(args);

    protected virtual void OnActivated() { }

    protected virtual void OnDeactivated() { }

    protected virtual void OnMouseLeftButtonDown(InputArgs args) { }

    protected virtual void OnMouseLeftButtonDragged(DragInputArgs args) { }

    protected virtual void OnMouseLeftButtonUp(InputArgs args) { }

    protected virtual void OnMouseRightButtonDown(InputArgs args) { }

    protected virtual void OnMouseRightButtonDragged(DragInputArgs args) { }

    protected virtual void OnMouseRightButtonUp(InputArgs args) { }

    protected virtual void OnMouseMiddleButtonDown(InputArgs args) { }

    protected virtual void OnMouseMiddleButtonDragged(DragInputArgs args) { }

    protected virtual void OnMouseMiddleButtonUp(InputArgs args) { }

    protected virtual void OnMouseMove(InputArgs args) { }

    protected virtual void OnMouseWheel(WheelInputArgs args) { }

    protected virtual void OnKeyDown(KeyInputArgs args) { }

    protected virtual void OnKeyUp(KeyInputArgs args) { }
}
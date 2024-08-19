namespace LogicSimulator.Models.Input;

public interface IInputTarget
{
    void MouseLeftButtonDown(InputArgs args);

    void MouseLeftButtonDragged(DragInputArgs args);

    void MouseLeftButtonUp(InputArgs args);

    void MouseRightButtonDown(InputArgs args);

    void MouseRightButtonDragged(DragInputArgs args);

    void MouseRightButtonUp(InputArgs args);

    void MouseMiddleButtonDown(InputArgs args);

    void MouseMiddleButtonDragged(DragInputArgs args);

    void MouseMiddleButtonUp(InputArgs args);

    void MouseMove(InputArgs args);

    void MouseWheel(WheelInputArgs args);

    void KeyDown(KeyInputArgs args);

    void KeyUp(KeyInputArgs args);
}

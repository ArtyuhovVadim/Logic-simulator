namespace LogicSimulator.Scene;

public static class RenderNotifier
{
    private static readonly Dictionary<Scene2D, bool> RenderRequiredDictionary = new();

    public static void RegisterScene(Scene2D scene)
    {
        RenderRequiredDictionary[scene] = false;
    }

    public static void RequestRender(Scene2D scene)
    {
        RenderRequiredDictionary[scene] = true;
    }

    public static bool IsRenderRequired(Scene2D scene) => RenderRequiredDictionary[scene];

    public static void RenderEnd(Scene2D scene) => RenderRequiredDictionary[scene] = false;
}
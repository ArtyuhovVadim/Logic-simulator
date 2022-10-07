using System.Collections.Generic;
using System.Linq;

namespace LogicSimulator.Scene;

public static class RenderNotifier
{
    private static readonly Dictionary<Scene2D, bool> RenderRequiredDictionary = new();

    public static void RegisterScene(Scene2D scene)
    {
        RenderRequiredDictionary[scene] = false;
    }

    public static void RequestRender(ResourceDependentObject obj)
    {
        if (RenderRequiredDictionary.Values.All(x => x)) return;

        foreach (var (scene, renderRequired) in RenderRequiredDictionary)
        {
            if (renderRequired)
                continue;

            if (scene.Objects.Any(x => x.Id == obj.Id))
            {
                RenderRequiredDictionary[scene] = true;
                return;
            }

            if (scene.Components.Any(x => x.Id == obj.Id))
            {
                RenderRequiredDictionary[scene] = true;
                return;
            }
        }

        if (RenderRequiredDictionary.Values.All(x => !x))
        {
            foreach (var key in RenderRequiredDictionary.Keys)
            {
                RenderRequiredDictionary[key] = true;
            }
        }
    }

    public static void RequestRender(Scene2D scene)
    {
        RenderRequiredDictionary[scene] = true;
    }

    public static bool IsRenderRequired(Scene2D scene) => RenderRequiredDictionary[scene];

    public static bool RenderEnd(Scene2D scene) => RenderRequiredDictionary[scene] = false;
}
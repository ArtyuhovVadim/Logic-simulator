using System.Collections.Generic;
using System.Linq;

namespace LogicSimulator.Scene;

public static class RenderNotifier
{
    private static readonly Dictionary<Scene2D, bool> RenderRequiredDictionary = new();

    //TODO: Вызывается после создания первых объектов, что пораждает ошибку в ResourceCache (ресурсы не обновляются, так как нет запроса на рендеринг). Исправил в ResourceCache костылем
    public static void RegisterScene(Scene2D scene)
    {
        RenderRequiredDictionary[scene] = false;
    }

    public static void RequestRender(ResourceDependentObject obj)
    {
        if (RenderRequiredDictionary.Values.All(x => x)) return;

        foreach (var (scene, renderRequired) in RenderRequiredDictionary)
        {
            if (renderRequired) continue;

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
    }

    public static bool IsRenderRequired(Scene2D scene) => RenderRequiredDictionary[scene];

    public static bool RenderEnd(Scene2D scene) => RenderRequiredDictionary[scene] = false;
}
using System.Windows;
using System.Windows.Automation;

namespace LogicSimulator.Utils;

public static class UiElementExtensionMethods
{
    public static T GetTemplateChildOrThrowIfNull<T>(this UIElement owner, DependencyObject obj) where T : UIElement
    {
        if (obj is null)
        {
            throw new ElementNotAvailableException($"Part element is not available in {owner.GetType()} template!");
        }

        return (T)obj;
    }


    public static void ThrowIfPartElementIsNull(this UIElement uiElement, UIElement owner)
    {
        if (uiElement is null)
        {
            throw new ElementNotAvailableException($"Part element is not available in {owner.GetType()} template!");
        }
    }
}
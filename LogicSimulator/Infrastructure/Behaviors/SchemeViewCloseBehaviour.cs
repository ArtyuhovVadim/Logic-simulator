using LogicSimulator.Models.Common;
using LogicSimulator.Scene;
using LogicSimulator.Views.Anchorable;
using Microsoft.Xaml.Behaviors;
using WpfExtensions.Utils;

namespace LogicSimulator.Infrastructure.Behaviors;

public class SchemeViewCloseBehaviour : Behavior<SchemeView>
{
    protected override void OnAttached()
    {
        if (AssociatedObject.DataContext is ICloseable closable)
        {
            closable.Closed += OnClosed;
        }
    }

    private void OnClosed()
    {
        ((ICloseable)AssociatedObject.DataContext).Closed -= OnClosed;
        var scene = AssociatedObject.FindVisualChild<Scene2D>()!;
        scene.Dispose();
    }
}
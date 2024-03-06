using Microsoft.Xaml.Behaviors;
using LogicSimulator.Views;
using WpfExtensions.Utils;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.AnchorableViewModels;

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
using System.Windows;
using AvalonDock.Controls;
using LogicSimulator.Views;
using Microsoft.Xaml.Behaviors;
using WpfExtensions.Utils;

namespace LogicSimulator.Infrastructure.Behaviors;

public class SchemeViewActivationBehaviour : Behavior<SchemeView>
{
    #region IsDocumentActive

    public bool IsDocumentActive
    {
        get => (bool)GetValue(IsDocumentActiveProperty);
        set => SetValue(IsDocumentActiveProperty, value);
    }

    public static readonly DependencyProperty IsDocumentActiveProperty =
        DependencyProperty.Register(nameof(IsDocumentActive), typeof(bool), typeof(SchemeViewActivationBehaviour), new PropertyMetadata(default(bool)));

    #endregion

    protected override void OnAttached()
    {
        AssociatedObject.PreviewMouseDown += OnMouseDown;
        AssociatedObject.MouseWheel += OnMouseWheel;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.PreviewMouseDown -= OnMouseDown;
        AssociatedObject.MouseWheel -= OnMouseWheel;
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        AssociatedObject.FindVisualParent<LayoutDocumentControl>()!.Model!.IsActive = true;
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (!IsDocumentActive && e.LeftButton == MouseButtonState.Pressed)
        {
            e.Handled = true;
        }

        AssociatedObject.FindVisualParent<LayoutDocumentControl>()!.Model!.IsActive = true;
    }
}
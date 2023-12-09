using System.Reflection;
using System.Windows;
using AvalonDock.Controls;
using Microsoft.Xaml.Behaviors;
using LogicSimulator.Views;
using WpfExtensions.Utils;
using AvalonDock.Layout;
using LogicSimulator.Scene;

namespace LogicSimulator.Infrastructure.Behaviors;

public class SchemeViewCloseBehaviour : Behavior<SchemeView>
{
    #region CloseCommand

    public ICommand CloseCommand
    {
        get => (ICommand)GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    public static readonly DependencyProperty CloseCommandProperty =
        DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(SchemeViewCloseBehaviour), new PropertyMetadata(default(ICommand)));

    #endregion

    protected override void OnAttached()
    {
        var control = AssociatedObject.FindVisualParent<LayoutDocumentControl>();
        var layoutContent = control!.Model!;
        var layoutItem = control.LayoutItem;

        layoutContent.Closed += OnClosed;

        if (typeof(LayoutItem).GetField("_defaultCloseCommand", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(layoutItem) is not ICommand defaultCommand)
            throw new ApplicationException("Can not find default close command.");

        layoutItem.CloseCommand = defaultCommand;
    }

    private void OnClosed(object? sender, EventArgs eventArgs)
    {
        ((LayoutContent)sender!).Closing -= OnClosed;
        CloseCommand?.Execute(null);
        var scene = AssociatedObject.FindVisualChild<Scene2D>()!;
        scene.Dispose();
    }
}
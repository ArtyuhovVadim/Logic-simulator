using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace LogicSimulator.Infrastructure.Behaviors;

public class ScrollViewerScrollToOffsetBehaviour : Behavior<ScrollViewer>
{
    #region VerticalOffset

    public double VerticalOffset
    {
        get => (double)GetValue(VerticalOffsetProperty);
        set => SetValue(VerticalOffsetProperty, value);
    }

    public static readonly DependencyProperty VerticalOffsetProperty =
        DependencyProperty.Register(nameof(VerticalOffset), typeof(double), typeof(ScrollViewerScrollToOffsetBehaviour), new PropertyMetadata(default(double), OnVerticalOffsetPropertyChanged));

    private static void OnVerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ScrollViewerScrollToOffsetBehaviour b) return;
        b.AssociatedObject?.ScrollToVerticalOffset((double)e.NewValue);
    }

    #endregion

    #region HorizontalOffset

    public double HorizontalOffset
    {
        get => (double)GetValue(HorizontalOffsetProperty);
        set => SetValue(HorizontalOffsetProperty, value);
    }

    public static readonly DependencyProperty HorizontalOffsetProperty =
        DependencyProperty.Register(nameof(HorizontalOffset), typeof(double), typeof(ScrollViewerScrollToOffsetBehaviour), new PropertyMetadata(default(double), OnHorizontalOffsetPropertyChanged));

    private static void OnHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ScrollViewerScrollToOffsetBehaviour b) return;
        b.AssociatedObject?.ScrollToHorizontalOffset((double)e.NewValue);
    }

    #endregion

    protected override void OnAttached()
    {
        AssociatedObject.ScrollChanged += OnScrollChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.ScrollChanged -= OnScrollChanged;
    }

    private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        VerticalOffset = e.VerticalOffset;
        HorizontalOffset = e.HorizontalOffset;
    }
}
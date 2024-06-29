using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LogicSimulator.Views;
using Microsoft.Xaml.Behaviors;

namespace LogicSimulator.Infrastructure.Behaviors;

public class TimelineScrollAndPanBehaviour : Behavior<TimelineView>
{
    #region WavesScrollViewer

    public ScrollViewer WavesScrollViewer
    {
        get => (ScrollViewer)GetValue(WavesScrollViewerProperty);
        set => SetValue(WavesScrollViewerProperty, value);
    }

    public static readonly DependencyProperty WavesScrollViewerProperty =
        DependencyProperty.Register(nameof(WavesScrollViewer), typeof(ScrollViewer), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(default(ScrollViewer)));

    #endregion

    #region SignalsScrollViewer

    public ScrollViewer SignalsScrollViewer
    {
        get => (ScrollViewer)GetValue(SignalsScrollViewerProperty);
        set => SetValue(SignalsScrollViewerProperty, value);
    }

    public static readonly DependencyProperty SignalsScrollViewerProperty =
        DependencyProperty.Register(nameof(SignalsScrollViewer), typeof(ScrollViewer), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(default(ScrollViewer)));

    #endregion

    #region HorizontalScrollBar

    public ScrollBar? HorizontalScrollBar
    {
        get => (ScrollBar)GetValue(HorizontalScrollBarProperty);
        set => SetValue(HorizontalScrollBarProperty, value);
    }

    public static readonly DependencyProperty HorizontalScrollBarProperty =
        DependencyProperty.Register(nameof(HorizontalScrollBar), typeof(ScrollBar), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(default(ScrollBar)));

    #endregion

    #region VerticalScrollBar

    public ScrollBar? VerticalScrollBar
    {
        get => (ScrollBar)GetValue(VerticalScrollBarProperty);
        set => SetValue(VerticalScrollBarProperty, value);
    }

    public static readonly DependencyProperty VerticalScrollBarProperty =
        DependencyProperty.Register(nameof(VerticalScrollBar), typeof(ScrollBar), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(default(ScrollBar)));

    #endregion

    #region HorizontalPanKeyModifier

    public ModifierKeys HorizontalPanKeyModifier
    {
        get => (ModifierKeys)GetValue(HorizontalPanKeyModifierProperty);
        set => SetValue(HorizontalPanKeyModifierProperty, value);
    }

    public static readonly DependencyProperty HorizontalPanKeyModifierProperty =
        DependencyProperty.Register(nameof(HorizontalPanKeyModifier), typeof(ModifierKeys), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(ModifierKeys.Shift));

    #endregion

    #region ZoomKeyModifier

    public ModifierKeys ZoomKeyModifier
    {
        get => (ModifierKeys)GetValue(ZoomKeyModifierProperty);
        set => SetValue(ZoomKeyModifierProperty, value);
    }

    public static readonly DependencyProperty ZoomKeyModifierProperty =
        DependencyProperty.Register(nameof(ZoomKeyModifier), typeof(ModifierKeys), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(ModifierKeys.Control));

    #endregion

    #region WavesCount

    public int WavesCount
    {
        get => (int)GetValue(WavesCountProperty);
        set => SetValue(WavesCountProperty, value);
    }

    public static readonly DependencyProperty WavesCountProperty =
        DependencyProperty.Register(nameof(WavesCount), typeof(int), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(default(int), OnVerticalScrollBarPropertyChanged));

    #endregion

    #region SignalHeight

    public double SignalHeight
    {
        get => (double)GetValue(SignalHeightProperty);
        set => SetValue(SignalHeightProperty, value);
    }

    public static readonly DependencyProperty SignalHeightProperty =
        DependencyProperty.Register(nameof(SignalHeight), typeof(double), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(0d, OnVerticalScrollBarPropertyChanged));

    #endregion

    #region MaxSignalsTime

    public ulong MaxSignalsTime
    {
        get => (ulong)GetValue(MaxSignalsTimeProperty);
        set => SetValue(MaxSignalsTimeProperty, value);
    }

    public static readonly DependencyProperty MaxSignalsTimeProperty =
        DependencyProperty.Register(nameof(MaxSignalsTime), typeof(ulong), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(default(ulong), OnHorizontalScrollBarPropertyChanged));

    #endregion

    #region Scale

    public double Scale
    {
        get => (double)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    public static readonly DependencyProperty ScaleProperty =
        DependencyProperty.Register(nameof(Scale), typeof(double), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(1d, OnHorizontalScrollBarPropertyChanged));

    #endregion

    #region HorizontalOffset

    public double HorizontalOffset
    {
        get => (double)GetValue(HorizontalOffsetProperty);
        set => SetValue(HorizontalOffsetProperty, value);
    }

    public static readonly DependencyProperty HorizontalOffsetProperty =
        DependencyProperty.Register(nameof(HorizontalOffset), typeof(double), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(0d));

    #endregion

    #region VerticalOffset

    public double VerticalOffset
    {
        get => (double)GetValue(VerticalOffsetProperty);
        set => SetValue(VerticalOffsetProperty, value);
    }

    public static readonly DependencyProperty VerticalOffsetProperty =
        DependencyProperty.Register(nameof(VerticalOffset), typeof(double), typeof(TimelineScrollAndPanBehaviour), new PropertyMetadata(0d));

    #endregion

    protected override void OnAttached()
    {
        AssociatedObject.Loaded += OnLoaded;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Loaded -= OnLoaded;
        AssociatedObject.SizeChanged -= OnSizeChanged;
        WavesScrollViewer.SizeChanged -= OnSizeChanged;
        WavesScrollViewer.PreviewMouseWheel -= OnWavesScrollViewerMouseWheel;
        SignalsScrollViewer.PreviewMouseWheel -= OnSignalsScrollViewerMouseWheel;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        WavesScrollViewer.PreviewMouseWheel += OnWavesScrollViewerMouseWheel;
        SignalsScrollViewer.PreviewMouseWheel += OnSignalsScrollViewerMouseWheel;
        AssociatedObject.SizeChanged += OnSizeChanged;
        WavesScrollViewer.SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateHorizontalScrollBarProperties();
        UpdateVerticalScrollBarProperties();
    }

    private void OnWavesScrollViewerMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var pos = e.GetPosition((IInputElement)sender).X;
        var zoomDelta = e.Delta;

        if (Keyboard.Modifiers == HorizontalPanKeyModifier)
        {
            HorizontalOffset += zoomDelta * Scale / 3d;
        }
        else if (Keyboard.Modifiers == ZoomKeyModifier)
        {
            var delta = zoomDelta * Scale / 1200;
            RelativeScale(delta, pos);
        }
        else if (Keyboard.Modifiers == ModifierKeys.None)
        {
            VerticalOffset = Math.Clamp(VerticalOffset - Math.Sign(zoomDelta) * SignalHeight, VerticalScrollBar!.Minimum, VerticalScrollBar!.Maximum);
        }

        e.Handled = true;
    }

    private void OnSignalsScrollViewerMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.None)
        {
            var zoomDelta = e.Delta;
            VerticalOffset = Math.Clamp(VerticalOffset - Math.Sign(zoomDelta) * SignalHeight, VerticalScrollBar!.Minimum, VerticalScrollBar!.Maximum);
        }

        e.Handled = true;
    }

    private static void OnHorizontalScrollBarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TimelineScrollAndPanBehaviour b) return;
        b.UpdateHorizontalScrollBarProperties();
    }

    private static void OnVerticalScrollBarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TimelineScrollAndPanBehaviour b) return;
        b.UpdateVerticalScrollBarProperties();
    }

    private void RelativeScale(double delta, double position, double min = 0.125, double max = 15)
    {
        var pos = (position - HorizontalOffset) / Scale;
        var newScaleCoefficient = 1 + delta / Scale;
        var newScale = Scale * newScaleCoefficient;
        if (newScale < min || newScale > max) return;
        HorizontalOffset += pos * ((1 - newScaleCoefficient) * Scale);
        Scale = newScale;
    }

    private void UpdateHorizontalScrollBarProperties()
    {
        if (HorizontalScrollBar is null) return;

        HorizontalScrollBar.Minimum = -WavesScrollViewer.ActualWidth;
        HorizontalScrollBar.Maximum = MaxSignalsTime * Scale;
        HorizontalScrollBar.ViewportSize = WavesScrollViewer.ActualWidth;
    }

    private void UpdateVerticalScrollBarProperties()
    {
        if (VerticalScrollBar is null) return;

        VerticalScrollBar.Minimum = 0;
        VerticalScrollBar.Maximum = WavesCount * SignalHeight - WavesScrollViewer.ActualHeight;
        VerticalScrollBar.ViewportSize = WavesScrollViewer.ActualHeight;
    }
}
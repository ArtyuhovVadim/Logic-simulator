using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LogicSimulator.Core;
using LogicSimulator.Shared.Models;

namespace LogicSimulator.Controls;

public class WaveView : Control
{
    private Pen? _lowSignalPen;
    private Pen? _highSignalPen;
    private Pen? _undefinedSignalPen;
    private Pen? _posEdgeSignalPen;
    private Pen? _negEdgeSignalPen;
    private Pen? _highImpSignalPen;

    #region Wave

    public IWave? Wave
    {
        get => (IWave)GetValue(WaveProperty);
        set => SetValue(WaveProperty, value);
    }

    public static readonly DependencyProperty WaveProperty =
        DependencyProperty.Register(nameof(Wave), typeof(IWave), typeof(WaveView), new FrameworkPropertyMetadata(default(IWave), FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region Scale

    public double Scale
    {
        get => (double)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    public static readonly DependencyProperty ScaleProperty =
        DependencyProperty.Register(nameof(Scale), typeof(double), typeof(WaveView), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region Offset

    public double Offset
    {
        get => (double)GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public static readonly DependencyProperty OffsetProperty =
        DependencyProperty.Register(nameof(Offset), typeof(double), typeof(WaveView), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region StrokeThickness

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(WaveView), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender, OnStrokeThicknessPropertyChanged));

    private static void OnStrokeThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not WaveView view) return;
        
        var newThickness = (double)e.NewValue;

        view._lowSignalPen = view.UpdatePen(view.LowSignalBrush, newThickness);
        view._highSignalPen = view.UpdatePen(view.HighSignalBrush, newThickness);
        view._undefinedSignalPen = view.UpdatePen(view.UndefinedSignalBrush, newThickness);
        view._posEdgeSignalPen = view.UpdatePen(view.PosEdgeSignalBrush, newThickness);
        view._negEdgeSignalPen = view.UpdatePen(view.NegEdgeSignalBrush, newThickness);
        view._highImpSignalPen = view.UpdatePen(view.HighImpSignalBrush, newThickness);
    }

    #endregion

    #region LowSignalBrush

    public Brush LowSignalBrush
    {
        get => (Brush)GetValue(LowSignalBrushProperty);
        set => SetValue(LowSignalBrushProperty, value);
    }

    public static readonly DependencyProperty LowSignalBrushProperty =
        DependencyProperty.Register(nameof(LowSignalBrush), typeof(Brush), typeof(WaveView), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 127, 0)), FrameworkPropertyMetadataOptions.AffectsRender, OnLowSignalBrushPropertyChanged));

    private static void OnLowSignalBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not WaveView view) return;
        view._lowSignalPen = view.UpdatePen(view.LowSignalBrush, view.StrokeThickness);
    }

    #endregion

    #region HighSignalBrush

    public Brush HighSignalBrush
    {
        get => (Brush)GetValue(HighSignalBrushProperty);
        set => SetValue(HighSignalBrushProperty, value);
    }

    public static readonly DependencyProperty HighSignalBrushProperty =
        DependencyProperty.Register(nameof(HighSignalBrush), typeof(Brush), typeof(WaveView), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 255, 0)), FrameworkPropertyMetadataOptions.AffectsRender, OnHighSignalBrushPropertyChanged));

    private static void OnHighSignalBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not WaveView view) return;
        view._highSignalPen = view.UpdatePen(view.HighSignalBrush, view.StrokeThickness);
    }

    #endregion

    #region UndefinedSignalBrush

    public Brush UndefinedSignalBrush
    {
        get => (Brush)GetValue(UndefinedSignalBrushProperty);
        set => SetValue(UndefinedSignalBrushProperty, value);
    }

    public static readonly DependencyProperty UndefinedSignalBrushProperty =
        DependencyProperty.Register(nameof(UndefinedSignalBrush), typeof(Brush), typeof(WaveView), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 127, 0)), FrameworkPropertyMetadataOptions.AffectsRender, OnUndefinedSignalBrushPropertyChanged));

    private static void OnUndefinedSignalBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not WaveView view) return;
        view._undefinedSignalPen = view.UpdatePen(view.UndefinedSignalBrush, view.StrokeThickness);
    }

    #endregion

    #region PosEdgeSignalBrush

    public Brush PosEdgeSignalBrush
    {
        get => (Brush)GetValue(PosEdgeSignalBrushProperty);
        set => SetValue(PosEdgeSignalBrushProperty, value);
    }

    public static readonly DependencyProperty PosEdgeSignalBrushProperty =
        DependencyProperty.Register(nameof(PosEdgeSignalBrush), typeof(Brush), typeof(WaveView), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(127, 0, 127)), FrameworkPropertyMetadataOptions.AffectsRender, OnPosEdgeSignalBrushPropertyChanged));

    private static void OnPosEdgeSignalBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not WaveView view) return;
        view._posEdgeSignalPen = view.UpdatePen(view.PosEdgeSignalBrush, view.StrokeThickness);
    }

    #endregion

    #region NegEdgeSignalBrush

    public Brush NegEdgeSignalBrush
    {
        get => (Brush)GetValue(NegEdgeSignalBrushProperty);
        set => SetValue(NegEdgeSignalBrushProperty, value);
    }

    public static readonly DependencyProperty NegEdgeSignalBrushProperty =
        DependencyProperty.Register(nameof(NegEdgeSignalBrush), typeof(Brush), typeof(WaveView), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 127)), FrameworkPropertyMetadataOptions.AffectsRender, OnNegEdgeSignalBrushPropertyChanged));

    private static void OnNegEdgeSignalBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not WaveView view) return;
        view._negEdgeSignalPen = view.UpdatePen(view.NegEdgeSignalBrush, view.StrokeThickness);
    }

    #endregion

    #region HighImpSignalBrush

    public Brush HighImpSignalBrush
    {
        get => (Brush)GetValue(HighImpSignalBrushProperty);
        set => SetValue(HighImpSignalBrushProperty, value);
    }

    public static readonly DependencyProperty HighImpSignalBrushProperty =
        DependencyProperty.Register(nameof(HighImpSignalBrush), typeof(Brush), typeof(WaveView), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), FrameworkPropertyMetadataOptions.AffectsRender, OnHighImpSignalBrushPropertyChanged));

    private static void OnHighImpSignalBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not WaveView view) return;
        view._highImpSignalPen = view.UpdatePen(view.HighImpSignalBrush, view.StrokeThickness);
    }

    #endregion

    protected override void OnRender(DrawingContext drawingContext)
    {
        if (Wave is null)
            return;

        if (_lowSignalPen is null)
            InvalidatePens();

        var size = new Size(ActualWidth, ActualHeight);
        var scale = Scale;
        var offset = Offset;
        var thickness = StrokeThickness;
        var halfThickness = thickness / 2;

        VisualXSnappingGuidelines = [];
        VisualYSnappingGuidelines = [];

        VisualXSnappingGuidelines.Add(size.Width + halfThickness);
        VisualXSnappingGuidelines.Add(size.Width - halfThickness);
        VisualXSnappingGuidelines.Add(halfThickness);
        VisualXSnappingGuidelines.Add(-halfThickness);

        VisualYSnappingGuidelines.Add(size.Height + halfThickness);
        VisualYSnappingGuidelines.Add(size.Height - halfThickness);
        VisualYSnappingGuidelines.Add(halfThickness);
        VisualYSnappingGuidelines.Add(-halfThickness);

        IWaveState? previousState = null;

        foreach (var state in Wave.States)
        {
            if (previousState is null)
            {
                previousState = state;
                continue;
            }

            var currentStateTime = Math.Min(size.Width, state.Time * scale + offset);
            var previousStateTime = Math.Max(0, previousState.Time * scale + offset);

            if (currentStateTime < 0)
            {
                previousState = state;
                continue;
            }

            if (previousStateTime > size.Width)
                break;

            var currentStateSignal = state.State;
            var previousStateSignal = previousState.State;

            OnRenderState(drawingContext, halfThickness, size, previousStateTime, previousStateSignal, currentStateTime, currentStateSignal);

            previousState = state;
        }

        base.OnRender(drawingContext);
    }

    private void OnRenderState(DrawingContext context, double halfStrokeThickness, Size size, double currentStateTime, SignalType currentStateSignal, double nextStateTime, SignalType nextStateSignal)
    {
        switch (currentStateSignal)
        {
            case SignalType.Low:
                {
                    var p0 = new Point(currentStateTime, size.Height);
                    var p1 = new Point(nextStateTime, size.Height);
                    context.DrawLine(_lowSignalPen, p0, p1);
                    RenderSignalTransition(context, halfStrokeThickness, size, p0, currentStateSignal, nextStateTime, nextStateSignal);
                }
                break;

            case SignalType.High:
                {
                    var p0 = new Point(currentStateTime, 0);
                    var p1 = new Point(nextStateTime, 0);
                    context.DrawLine(_highSignalPen, p0, p1);
                    RenderSignalTransition(context, halfStrokeThickness, size, p0, currentStateSignal, nextStateTime, nextStateSignal);
                }
                break;

            case SignalType.Undefined:
                {
                    var p0 = new Point(currentStateTime, size.Height / 2);
                    var p1 = new Point(nextStateTime, size.Height / 2);
                    context.DrawLine(_undefinedSignalPen, p0, p1);
                    RenderSignalTransition(context, halfStrokeThickness, size, p0, currentStateSignal, nextStateTime, nextStateSignal);
                }
                break;

            case SignalType.PosEdge:
                break;

            case SignalType.NegEdge:
                break;

            case SignalType.HighImp:
                {
                    var p0 = new Point(currentStateTime, size.Height / 2);
                    var p1 = new Point(nextStateTime, size.Height / 2);
                    context.DrawLine(_highImpSignalPen, p0, p1);
                    RenderSignalTransition(context, halfStrokeThickness, size, p0, currentStateSignal, nextStateTime, nextStateSignal);
                }
                break;
        }
    }

    private void RenderSignalTransition(DrawingContext context, double halfStrokeThickness, Size size, Point currentStatePoint, SignalType currentStateSignal, double nextStateTime, SignalType nextStateSignal)
    {
        if (currentStateSignal == nextStateSignal)
            return;

        switch (nextStateSignal)
        {
            case SignalType.Low:
                context.DrawLine(_lowSignalPen, currentStatePoint with { X = nextStateTime }, new Point(nextStateTime, size.Height));
                break;

            case SignalType.High:
                context.DrawLine(_highSignalPen, new Point(nextStateTime, 0), currentStatePoint with { X = nextStateTime });
                break;

            case SignalType.Undefined:
                context.DrawLine(_undefinedSignalPen, new Point(nextStateTime, size.Height / 2), currentStatePoint with { X = nextStateTime });
                break;

            case SignalType.PosEdge:
                break;

            case SignalType.NegEdge:
                break;

            case SignalType.HighImp:
                context.DrawLine(_highImpSignalPen, new Point(nextStateTime, size.Height / 2), currentStatePoint with { X = nextStateTime });
                break;
        }

        VisualXSnappingGuidelines.Add(nextStateTime - halfStrokeThickness);
        VisualXSnappingGuidelines.Add(nextStateTime + halfStrokeThickness);
    }

    private Pen UpdatePen(Brush brush, double thickness)
    {
        var pen = new Pen(brush, thickness);
        pen.Freeze();
        return pen;
    }

    private void InvalidatePens()
    {
        _lowSignalPen = UpdatePen(LowSignalBrush, StrokeThickness);
        _highSignalPen = UpdatePen(HighSignalBrush, StrokeThickness);
        _undefinedSignalPen = UpdatePen(UndefinedSignalBrush, StrokeThickness);
        _posEdgeSignalPen = UpdatePen(PosEdgeSignalBrush, StrokeThickness);
        _negEdgeSignalPen = UpdatePen(NegEdgeSignalBrush, StrokeThickness);
        _highImpSignalPen = UpdatePen(HighImpSignalBrush, StrokeThickness);
    }
}
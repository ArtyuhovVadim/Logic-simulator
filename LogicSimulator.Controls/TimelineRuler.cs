using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogicSimulator.Controls;

public class TimelineRuler : Control
{
    #region Scale

    public double Scale
    {
        get => (double)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    public static readonly DependencyProperty ScaleProperty =
        DependencyProperty.Register(nameof(Scale), typeof(double), typeof(TimelineRuler), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region Offset

    public double Offset
    {
        get => (double)GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public static readonly DependencyProperty OffsetProperty =
        DependencyProperty.Register(nameof(Offset), typeof(double), typeof(TimelineRuler), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region Frequency

    public double Frequency
    {
        get => (double)GetValue(FrequencyProperty);
        set => SetValue(FrequencyProperty, value);
    }

    public static readonly DependencyProperty FrequencyProperty =
        DependencyProperty.Register(nameof(Frequency), typeof(double), typeof(TimelineRuler), new FrameworkPropertyMetadata(10d, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region AlternationCount

    public int AlternationCount
    {
        get => (int)GetValue(AlternationCountProperty);
        set => SetValue(AlternationCountProperty, value);
    }

    public static readonly DependencyProperty AlternationCountProperty =
        DependencyProperty.Register(nameof(AlternationCount), typeof(int), typeof(TimelineRuler), new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region TickHeight

    public double TickHeight
    {
        get => (double)GetValue(TickHeightProperty);
        set => SetValue(TickHeightProperty, value);
    }

    public static readonly DependencyProperty TickHeightProperty =
        DependencyProperty.Register(nameof(TickHeight), typeof(double), typeof(TimelineRuler), new FrameworkPropertyMetadata(10d, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region StrokeBrush

    public Brush StrokeBrush
    {
        get => (Brush)GetValue(StrokeBrushProperty);
        set => SetValue(StrokeBrushProperty, value);
    }

    public static readonly DependencyProperty StrokeBrushProperty =
        DependencyProperty.Register(nameof(StrokeBrush), typeof(Brush), typeof(TimelineRuler), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region StrokeThickness

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(TimelineRuler), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region TimeUnitSuffix

    public string TimeUnitSuffix
    {
        get => (string)GetValue(TimeUnitSuffixProperty);
        set => SetValue(TimeUnitSuffixProperty, value);
    }

    public static readonly DependencyProperty TimeUnitSuffixProperty =
        DependencyProperty.Register(nameof(TimeUnitSuffix), typeof(string), typeof(TimelineRuler), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    protected override void OnRender(DrawingContext drawingContext)
    {
        var strokeThickness = StrokeThickness;
        var frequency = Frequency;
        var scaledFrequency = Frequency * Scale;
        var offset = Offset;
        var size = new Size(ActualWidth, ActualHeight);
        var linesCount = (int)(size.Width / scaledFrequency + 1);
        var alternationCount = Math.Max(1, AlternationCount);
        var tickHeight = TickHeight;
        var suffix = TimeUnitSuffix;
        var guidelines = new DoubleCollection(linesCount);
        var pen = new Pen(StrokeBrush, strokeThickness);
        var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        var dpi = VisualTreeHelper.GetDpi(this);

        var maxTextHeight = 0d;
        var offsetInTicks = (int)(-offset / scaledFrequency);
        var alternationOffsetInTicks = (int)(-offset / scaledFrequency / alternationCount);

        for (var i = Math.Max(0, alternationOffsetInTicks); i < linesCount / alternationCount + alternationOffsetInTicks + 2; i++)
        {
            var x = i * alternationCount * scaledFrequency + offset;
            var text = new FormattedText($"{i * alternationCount * frequency}{suffix}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, FontSize, Foreground, dpi.DpiScaleX);
            maxTextHeight = Math.Max(maxTextHeight, text.Height);

            var p0 = new Point(x, 0);
            var p1 = new Point(x, size.Height);
            drawingContext.DrawLine(pen, p0, p1);
            drawingContext.DrawText(text, new Point(x - text.Width / 2, 0));
        }

        for (var i = Math.Max(0, offsetInTicks); i < linesCount + offsetInTicks; i++)
        {
            var x = i * scaledFrequency + offset;
            var p0 = new Point(x, maxTextHeight);
            var p1 = new Point(x, maxTextHeight + tickHeight);

            drawingContext.DrawLine(pen, p0, p1);
            guidelines.Add(x + strokeThickness / 2);
        }

        guidelines.Add(size.Width + strokeThickness / 2);
        guidelines.Add(size.Width + scaledFrequency + strokeThickness / 2);

        VisualXSnappingGuidelines = guidelines;
        base.OnRender(drawingContext);
    }
}
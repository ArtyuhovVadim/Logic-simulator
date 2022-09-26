using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using LogicSimulator.Extensions;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace LogicSimulator.Controls;

public class HueAdorner : Adorner
{
    #region Hue

    public double Hue
    {
        get => (double)GetValue(HueProperty);
        set => SetValue(HueProperty, value);
    }

    public static readonly DependencyProperty HueProperty =
        DependencyProperty.Register(nameof(Hue), typeof(double), typeof(HueAdorner), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender), OnHueValidateValue);

    private static bool OnHueValidateValue(object value)
    {
        if (value is not double o) return false;

        return o switch
        {
            < 0 => false,
            > 360 => false,
            _ => true
        };
    }

    #endregion

    private readonly Pen _blackPen = new(new SolidColorBrush(Color.FromRgb(34, 34, 34)), 1);
    private readonly SolidColorBrush _brush = new();

    private readonly UIElement _root;

    public HueAdorner(UIElement adornedElement) : base(adornedElement)
    {
        IsHitTestVisible = false;
        _root = adornedElement;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        var halfPenThickness = _blackPen.Thickness / 2;

        var size = new Size(4, 14);
        var location = new Point(Hue.Map(0, 360, -size.Width / 2, _root.RenderSize.Width), (_root.RenderSize.Height - size.Height) / 2);

        var rect = new Rect(location, size);
        rect = new Rect(rect.Left - halfPenThickness, rect.Top - halfPenThickness, rect.Width + _blackPen.Thickness, rect.Height + _blackPen.Thickness);

        _brush.Color = ColorHelper.ColorFromHsv(Hue, 1, 1);

        var guidelines = new GuidelineSet();

        guidelines.GuidelinesX.Add(rect.Left + halfPenThickness);
        guidelines.GuidelinesX.Add(rect.Right + halfPenThickness);
        guidelines.GuidelinesY.Add(rect.Top + halfPenThickness);
        guidelines.GuidelinesY.Add(rect.Bottom + halfPenThickness);

        drawingContext.PushGuidelineSet(guidelines);

        drawingContext.DrawRoundedRectangle(_brush, _blackPen, rect, 2, 2);
    }
}
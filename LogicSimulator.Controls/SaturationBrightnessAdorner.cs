using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace LogicSimulator.Controls;

public class SaturationBrightnessAdorner : Adorner
{
    #region Brightness

    public double Brightness
    {
        get => (double)GetValue(BrightnessProperty);
        set => SetValue(BrightnessProperty, value);
    }

    public static readonly DependencyProperty BrightnessProperty =
        DependencyProperty.Register(nameof(Brightness), typeof(double), typeof(SaturationBrightnessAdorner), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender), OnBrightnessValidateValue);

    private static bool OnBrightnessValidateValue(object value)
    {
        if (value is not double o) return false;

        return o switch
        {
            < 0 => false,
            > 1 => false,
            _ => true
        };
    }

    #endregion

    #region Saturation

    public double Saturation
    {
        get => (double)GetValue(SaturationProperty);
        set => SetValue(SaturationProperty, value);
    }

    public static readonly DependencyProperty SaturationProperty =
        DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(SaturationBrightnessAdorner), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender), OnSaturationValidateValue);

    private static bool OnSaturationValidateValue(object value)
    {
        if (value is not double o) return false;

        return o switch
        {
            < 0 => false,
            > 1 => false,
            _ => true
        };
    }

    #endregion

    private readonly Pen _whitePen = new(Brushes.White, 1);
    private readonly Pen _blackPen = new(Brushes.Black, 1);

    private readonly UIElement _root;

    public SaturationBrightnessAdorner(UIElement adornedElement) : base(adornedElement)
    {
        IsHitTestVisible = false;
        _root = adornedElement;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        var center = new Point(_root.RenderSize.Width * Saturation, _root.RenderSize.Height * (1d - Brightness));

        drawingContext.DrawEllipse(null, _whitePen, center, 3.5, 3.5);
        drawingContext.DrawEllipse(null, _blackPen, center, 4.5, 4.5);
    }
}
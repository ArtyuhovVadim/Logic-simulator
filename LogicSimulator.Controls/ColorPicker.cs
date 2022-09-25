using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using LogicSimulator.Extensions;
using SharpDX;

namespace LogicSimulator.Controls;

[TemplatePart(Name = RootPopupName, Type = typeof(Popup))]
[TemplatePart(Name = RootButtonName, Type = typeof(Button))]
[TemplatePart(Name = ApplyButtonName, Type = typeof(Button))]
[TemplatePart(Name = CancelButtonName, Type = typeof(Button))]
[TemplatePart(Name = HuePickerBorderName, Type = typeof(Border))]
[TemplatePart(Name = SaturationBrightnessBorderName, Type = typeof(Border))]
public class ColorPicker : Control
{
    #region Color

    public Color4 Color
    {
        get => (Color4)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(Color4), typeof(ColorPicker), new PropertyMetadata(default(Color4)));

    #endregion

    #region IsValueUndefined

    public bool IsValueUndefined
    {
        get => (bool)GetValue(IsValueUndefinedProperty);
        set => SetValue(IsValueUndefinedProperty, value);
    }

    public static readonly DependencyProperty IsValueUndefinedProperty =
        DependencyProperty.Register(nameof(IsValueUndefined), typeof(bool), typeof(ColorPicker), new PropertyMetadata(false));

    #endregion

    private const string RootPopupName = "PART_RootPopup";
    private const string RootButtonName = "PART_RootButton";
    private const string ApplyButtonName = "PART_ApplyButton";
    private const string CancelButtonName = "PART_CancelButton";
    private const string HuePickerBorderName = "PART_HuePickerBorder";
    private const string SaturationBrightnessBorderName = "PART_SaturationBrightnessBorder";

    private Popup _rootPopup;
    private Button _rootButton;

    private Button _applyButton;
    private Button _cancelButton;

    private Border _huePickerBorder;
    private Border _saturationBrightnessBorder;

    private HueAdorner _huePickerAdorner;
    private SaturationBrightnessAdorner _saturationBrightnessAdorner;

    static ColorPicker()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _rootPopup = GetTemplateChild(RootPopupName) as Popup;
        _rootButton = GetTemplateChild(RootButtonName) as Button;

        _applyButton = GetTemplateChild(ApplyButtonName) as Button;
        _cancelButton = GetTemplateChild(CancelButtonName) as Button;

        _huePickerBorder = GetTemplateChild(HuePickerBorderName) as Border;
        _saturationBrightnessBorder = GetTemplateChild(SaturationBrightnessBorderName) as Border;

        if (_rootPopup is null || _rootButton is null || _applyButton is null || _cancelButton is null || _huePickerBorder is null || _saturationBrightnessBorder is null)
            throw new ElementNotAvailableException($"Part element is not available in {GetType()} template!");

        _rootButton.Click += OnRootButtonClick;

        _applyButton.Click += OnApplyButtonClick;
        _cancelButton.Click += OnCancelButtonClick;

        _huePickerBorder.MouseLeftButtonDown += OnHuePickerBorderMouseLeftButtonDown;
        _huePickerBorder.MouseLeftButtonUp += OnHuePickerBorderMouseLeftButtonUp;
        _huePickerBorder.MouseMove += OnHuePickerBorderMouseMove;

        _saturationBrightnessBorder.MouseLeftButtonDown += OnSaturationBrightnessBorderMouseLeftButtonDown;
        _saturationBrightnessBorder.MouseLeftButtonUp += OnSaturationBrightnessBorderMouseLeftButtonUp;
        _saturationBrightnessBorder.MouseMove += OnSaturationBrightnessBorderMouseMove;

        _huePickerAdorner = new HueAdorner(_huePickerBorder);
        _saturationBrightnessAdorner = new SaturationBrightnessAdorner(_saturationBrightnessBorder);

        var huePickerLayer = AdornerLayer.GetAdornerLayer(_huePickerBorder);
        var saturationBrightnessLayer = AdornerLayer.GetAdornerLayer(_saturationBrightnessBorder);

        if (huePickerLayer is null || saturationBrightnessLayer is null)
            throw new ElementNotAvailableException("Adorner layer not found!");

        huePickerLayer.Add(_huePickerAdorner);
        saturationBrightnessLayer.Add(_saturationBrightnessAdorner);
    }

    private void OnSaturationBrightnessBorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        (_saturationBrightnessAdorner.Saturation, _saturationBrightnessAdorner.Brightness) = GetSaturationBrightnessValues(e);
    }

    private void OnSaturationBrightnessBorderMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            _saturationBrightnessBorder.CaptureMouse();
            (_saturationBrightnessAdorner.Saturation, _saturationBrightnessAdorner.Brightness) = GetSaturationBrightnessValues(e);
        }
    }

    private void OnSaturationBrightnessBorderMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        Mouse.Capture(null);
    }

    private void OnHuePickerBorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _huePickerAdorner.Hue = GetHueValue(e);
    }

    private void OnHuePickerBorderMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            _huePickerBorder.CaptureMouse();
            _huePickerAdorner.Hue = GetHueValue(e);
        }
    }

    private void OnHuePickerBorderMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        Mouse.Capture(null);
    }

    private void OnRootButtonClick(object sender, RoutedEventArgs e)
    {
        _rootPopup.IsOpen = true;
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        _rootPopup.IsOpen = false;
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        _rootPopup.IsOpen = false;
    }

    private double GetHueValue(MouseEventArgs e) => e.GetPosition(_huePickerBorder).X.Map(0, _huePickerBorder.RenderSize.Width, 0, 360).Clamp(0, 360);

    private (double saturation, double brightness) GetSaturationBrightnessValues(MouseEventArgs e)
    {
        var pos = e.GetPosition(_saturationBrightnessBorder);

        var saturation = pos.X.Map(0, _saturationBrightnessBorder.RenderSize.Width, 0, 1).Clamp(0, 1);
        var brightness = 1d - pos.Y.Map(0, _saturationBrightnessBorder.RenderSize.Height, 0, 1).Clamp(0, 1);

        return (saturation, brightness);
    }
}
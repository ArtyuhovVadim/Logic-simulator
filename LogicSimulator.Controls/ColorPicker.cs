using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using LogicSimulator.Utils;
using SharpDX;
using WpfColor = System.Windows.Media.Color;

namespace LogicSimulator.Controls;

[TemplatePart(Name = RootPopupName, Type = typeof(Popup))]
[TemplatePart(Name = RootButtonName, Type = typeof(Button))]
[TemplatePart(Name = ApplyButtonName, Type = typeof(Button))]
[TemplatePart(Name = CancelButtonName, Type = typeof(Button))]
[TemplatePart(Name = HuePickerBorderName, Type = typeof(Border))]
[TemplatePart(Name = SaturationBrightnessBorderName, Type = typeof(Border))]
[TemplatePart(Name = HexTextBoxExName, Type = typeof(TextBoxEx))]
[TemplatePart(Name = RedTextBoxExName, Type = typeof(TextBoxEx))]
[TemplatePart(Name = GreenTextBoxExName, Type = typeof(TextBoxEx))]
[TemplatePart(Name = BlueTextBoxExName, Type = typeof(TextBoxEx))]
public class ColorPicker : Control
{
    #region Color

    public Color4 Color
    {
        get => (Color4)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(Color4), typeof(ColorPicker), new FrameworkPropertyMetadata(Color4.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));

    private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ColorPicker colorPicker) return;

        var color = (Color4)e.NewValue;
        var hsv = color.AsHsv();

        colorPicker.TempColor = color.ToColor();

        colorPicker.Hue = hsv.h;
        colorPicker.Saturation = hsv.s;
        colorPicker.Brightness = hsv.v;

        colorPicker.TempHue = hsv.h;
        colorPicker._hue = hsv.h;
        colorPicker._saturation = hsv.s;
        colorPicker._brightness = hsv.v;

        colorPicker.UpdateRgbTextBoxes();

        if (colorPicker._huePickerAdorner is null || colorPicker._saturationBrightnessAdorner is null) return;

        colorPicker._huePickerAdorner.Hue = hsv.h;
        colorPicker._saturationBrightnessAdorner.Saturation = hsv.s;
        colorPicker._saturationBrightnessAdorner.Brightness = hsv.v;
    }

    #endregion

    #region TempColor

    public static readonly DependencyPropertyKey TempColorPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(TempColor), typeof(WpfColor), typeof(ColorPicker), new FrameworkPropertyMetadata(Colors.Black));

    public WpfColor TempColor
    {
        get => (WpfColor)GetValue(TempColorPropertyKey.DependencyProperty);
        private set => SetValue(TempColorPropertyKey, value);
    }

    #endregion

    #region Hue

    public static readonly DependencyPropertyKey HuePropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(Hue), typeof(double), typeof(ColorPicker), new FrameworkPropertyMetadata(default(double)));

    public double Hue
    {
        get => (double)GetValue(HuePropertyKey.DependencyProperty);
        private set => SetValue(HuePropertyKey, value);
    }

    #endregion

    #region TempHue

    internal static readonly DependencyPropertyKey TempHuePropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(TempHue), typeof(double), typeof(ColorPicker), new FrameworkPropertyMetadata(default(double)));

    internal double TempHue
    {
        get => (double)GetValue(TempHuePropertyKey.DependencyProperty);
        private set => SetValue(TempHuePropertyKey, value);
    }

    #endregion

    #region Saturation

    public static readonly DependencyPropertyKey SaturationPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(Saturation), typeof(double), typeof(ColorPicker), new FrameworkPropertyMetadata(default(double)));

    public double Saturation
    {
        get => (double)GetValue(SaturationPropertyKey.DependencyProperty);
        private set => SetValue(SaturationPropertyKey, value);
    }

    #endregion

    #region Brightness

    public static readonly DependencyPropertyKey BrightnessPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(Brightness), typeof(double), typeof(ColorPicker), new FrameworkPropertyMetadata(default(double)));

    public double Brightness
    {
        get => (double)GetValue(BrightnessPropertyKey.DependencyProperty);
        private set => SetValue(BrightnessPropertyKey, value);
    }

    #endregion

    #region IsValueUndefined

    public bool IsValueUndefined
    {
        get => (bool)GetValue(IsValueUndefinedProperty);
        set => SetValue(IsValueUndefinedProperty, value);
    }

    public static readonly DependencyProperty IsValueUndefinedProperty =
        DependencyProperty.Register(nameof(IsValueUndefined), typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    #endregion

    private const string RootPopupName = "PART_RootPopup";
    private const string RootButtonName = "PART_RootButton";
    private const string ApplyButtonName = "PART_ApplyButton";
    private const string CancelButtonName = "PART_CancelButton";
    private const string HuePickerBorderName = "PART_HuePickerBorder";
    private const string SaturationBrightnessBorderName = "PART_SaturationBrightnessBorder";
    private const string HexTextBoxExName = "PART_HexTextBoxEx";
    private const string RedTextBoxExName = "PART_RedTextBoxEx";
    private const string GreenTextBoxExName = "PART_GreenTextBoxEx";
    private const string BlueTextBoxExName = "PART_BlueTextBoxEx";

    private Popup _rootPopup;
    private Button _rootButton;

    private Button _applyButton;
    private Button _cancelButton;

    private Border _huePickerBorder;
    private Border _saturationBrightnessBorder;

    private HueAdorner _huePickerAdorner;
    private SaturationBrightnessAdorner _saturationBrightnessAdorner;

    private TextBoxEx _hexTextBox;
    private TextBoxEx _redTextBoxEx;
    private TextBoxEx _greenTextBoxEx;
    private TextBoxEx _blueTextBoxEx;

    private double _hue;
    private double _saturation;
    private double _brightness;

    static ColorPicker()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _rootPopup = this.GetTemplateChildOrThrowIfNull<Popup>(GetTemplateChild(RootPopupName));
        _rootButton = this.GetTemplateChildOrThrowIfNull<Button>(GetTemplateChild(RootButtonName));

        _applyButton = this.GetTemplateChildOrThrowIfNull<Button>(GetTemplateChild(ApplyButtonName));
        _cancelButton = this.GetTemplateChildOrThrowIfNull<Button>(GetTemplateChild(CancelButtonName));

        _huePickerBorder = this.GetTemplateChildOrThrowIfNull<Border>(GetTemplateChild(HuePickerBorderName));
        _saturationBrightnessBorder = this.GetTemplateChildOrThrowIfNull<Border>(GetTemplateChild(SaturationBrightnessBorderName));

        _hexTextBox = this.GetTemplateChildOrThrowIfNull<TextBoxEx>(GetTemplateChild(HexTextBoxExName));
        _redTextBoxEx = this.GetTemplateChildOrThrowIfNull<TextBoxEx>(GetTemplateChild(RedTextBoxExName));
        _greenTextBoxEx = this.GetTemplateChildOrThrowIfNull<TextBoxEx>(GetTemplateChild(GreenTextBoxExName));
        _blueTextBoxEx = this.GetTemplateChildOrThrowIfNull<TextBoxEx>(GetTemplateChild(BlueTextBoxExName));

        _rootButton.Click += OnRootButtonClick;
        _applyButton.Click += OnApplyButtonClick;
        _cancelButton.Click += OnCancelButtonClick;

        _huePickerBorder.MouseLeftButtonDown += OnHuePickerBorderMouseLeftButtonDown;
        _huePickerBorder.MouseLeftButtonUp += OnHuePickerBorderMouseLeftButtonUp;
        _huePickerBorder.MouseMove += OnHuePickerBorderMouseMove;

        _saturationBrightnessBorder.MouseLeftButtonDown += OnSaturationBrightnessBorderMouseLeftButtonDown;
        _saturationBrightnessBorder.MouseLeftButtonUp += OnSaturationBrightnessBorderMouseLeftButtonUp;
        _saturationBrightnessBorder.MouseMove += OnSaturationBrightnessBorderMouseMove;

        _hexTextBox.Confirm += OnHexTextBoxConfirm;

        _redTextBoxEx.Confirm += OnRgbTextBoxesConfirm;
        _greenTextBoxEx.Confirm += OnRgbTextBoxesConfirm;
        _blueTextBoxEx.Confirm += OnRgbTextBoxesConfirm;

        UpdateRgbTextBoxes();

        _huePickerAdorner = new HueAdorner(_huePickerBorder) { Hue = Hue };
        _saturationBrightnessAdorner = new SaturationBrightnessAdorner(_saturationBrightnessBorder) { Saturation = Saturation, Brightness = Brightness };

        var huePickerLayer = AdornerLayer.GetAdornerLayer(_huePickerBorder);
        var saturationBrightnessLayer = AdornerLayer.GetAdornerLayer(_saturationBrightnessBorder);

        if (huePickerLayer is null || saturationBrightnessLayer is null)
            throw new ElementNotAvailableException("Adorner layer not found!");

        huePickerLayer.Add(_huePickerAdorner);
        saturationBrightnessLayer.Add(_saturationBrightnessAdorner);
    }

    private void OnHexTextBoxConfirm(object sender, RoutedEventArgs e)
    {
        try
        {
            var color = (WpfColor)ColorConverter.ConvertFromString(_hexTextBox.Text)! with { A = 255 };
            _hexTextBox.IsHasError = false;
            TempColor = color;

            var hsv = TempColor.ToColor4().AsHsv();

            _huePickerAdorner.Hue = hsv.h;
            TempHue = hsv.h;
            _saturationBrightnessAdorner.Saturation = hsv.s;
            _saturationBrightnessAdorner.Brightness = hsv.v;

            UpdateRgbTextBoxes();
        }
        catch
        {
            _hexTextBox.IsHasError = true;
        }
    }

    private void OnRgbTextBoxesConfirm(object sender, RoutedEventArgs e)
    {
        var textBox = (TextBoxEx)sender;

        if (byte.TryParse(textBox.Text, out var result))
        {
            textBox.IsHasError = false;

            if (textBox == _redTextBoxEx) TempColor = TempColor with { R = result };
            else if (textBox == _greenTextBoxEx) TempColor = TempColor with { G = result };
            else TempColor = TempColor with { B = result };

            var color = TempColor.ToColor4().AsHsv();

            _huePickerAdorner.Hue = color.h;
            TempHue = color.h;
            _saturationBrightnessAdorner.Saturation = color.s;
            _saturationBrightnessAdorner.Brightness = color.v;
        }
        else
        {
            textBox.IsHasError = true;
        }
    }

    private void OnSaturationBrightnessBorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        (_saturationBrightnessAdorner.Saturation, _saturationBrightnessAdorner.Brightness) = GetSaturationBrightnessValues(e);
    }

    private void OnSaturationBrightnessBorderMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if (!_saturationBrightnessBorder.IsMouseCaptured)
            {
                _saturationBrightnessBorder.CaptureMouse();
            }

            (_saturationBrightnessAdorner.Saturation, _saturationBrightnessAdorner.Brightness) = GetSaturationBrightnessValues(e);
        }
    }

    private void OnSaturationBrightnessBorderMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_saturationBrightnessBorder.IsMouseCaptured)
        {
            Mouse.Capture(null);
        }
    }

    private void OnHuePickerBorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _huePickerAdorner.Hue = GetHueValue(e);
    }

    private void OnHuePickerBorderMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if (!_huePickerBorder.IsMouseCaptured)
            {
                _huePickerBorder.CaptureMouse();
            }

            _huePickerAdorner.Hue = GetHueValue(e);
        }
    }

    private void OnHuePickerBorderMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_huePickerBorder.IsMouseCaptured)
        {
            Mouse.Capture(null);
        }
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        Hue = _hue;
        Saturation = _saturation;
        Brightness = _brightness;

        IsValueUndefined = false;

        Color = TempColor.ToColor4();

        _rootPopup.IsOpen = false;
    }

    private void OnRootButtonClick(object sender, RoutedEventArgs e) => _rootPopup.IsOpen = true;

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        _hue = Hue;
        _saturation = Saturation;
        _brightness = Brightness;

        TempColor = Color.ToColor();

        var hsv = TempColor.ToColor4().AsHsv();

        _huePickerAdorner.Hue = hsv.h;
        TempHue = hsv.h;
        _saturationBrightnessAdorner.Saturation = hsv.s;
        _saturationBrightnessAdorner.Brightness = hsv.v;

        UpdateRgbTextBoxes();

        _rootPopup.IsOpen = false;
    }

    private double GetHueValue(MouseEventArgs e)
    {
        _hue = e.GetPosition(_huePickerBorder).X.Map(0, _huePickerBorder.RenderSize.Width, 0, 360).Clamp(0, 360);

        TempColor = ColorHelper.ColorFromHsv(_hue, _saturation, _brightness);

        TempHue = _hue;

        UpdateRgbTextBoxes();

        return _hue;
    }

    private (double saturation, double brightness) GetSaturationBrightnessValues(MouseEventArgs e)
    {
        var pos = e.GetPosition(_saturationBrightnessBorder);

        _saturation = pos.X.Map(0, _saturationBrightnessBorder.RenderSize.Width, 0, 1).Clamp(0, 1);
        _brightness = 1d - pos.Y.Map(0, _saturationBrightnessBorder.RenderSize.Height, 0, 1).Clamp(0, 1);

        TempColor = ColorHelper.ColorFromHsv(_hue, _saturation, _brightness);

        UpdateRgbTextBoxes();

        return (_saturation, _brightness);
    }

    private void UpdateRgbTextBoxes()
    {
        _redTextBoxEx.Text = TempColor.R.ToString();
        _greenTextBoxEx.Text = TempColor.G.ToString();
        _blueTextBoxEx.Text = TempColor.B.ToString();

        _hexTextBox.Text = "#" + Convert.ToHexString(new[] { TempColor.R, TempColor.G, TempColor.B });
    }
}
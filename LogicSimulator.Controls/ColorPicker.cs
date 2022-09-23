using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SharpDX;

namespace LogicSimulator.Controls;

[TemplatePart(Name = RootPopupName, Type = typeof(Popup))]
[TemplatePart(Name = RootButtonName, Type = typeof(Button))]
[TemplatePart(Name = ApplyButtonName, Type = typeof(Button))]
[TemplatePart(Name = CancelButtonName, Type = typeof(Button))]
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

    private Popup _rootPopup;
    private Button _rootButton;
    
    private Button _applyButton;
    private Button _cancelButton;

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

        if (_rootPopup is null || _rootButton is null || _applyButton is null || _cancelButton is null)
            throw new ElementNotAvailableException($"Part element is not available in {GetType()} template!");

        _rootButton.Click += OnRootButtonClick;
        _applyButton.Click += OnApplyButtonClick;
        _cancelButton.Click += OnCancelButtonClick;
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
}
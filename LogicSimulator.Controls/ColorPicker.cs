using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LogicSimulator.Controls;

[TemplatePart(Name = RootPopupName, Type = typeof(Popup))]
[TemplatePart(Name = RootButtonName, Type = typeof(Button))]
public class ColorPicker : Control
{
    private const string RootPopupName = "PART_RootPopup";
    private const string RootButtonName = "PART_RootButton";

    private Popup _rootPopup;
    private Button _rootButton;

    static ColorPicker()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _rootPopup = GetTemplateChild(RootPopupName) as Popup;
        _rootButton = GetTemplateChild(RootButtonName) as Button;

        if (_rootPopup is null || _rootButton is null)
            throw new ElementNotAvailableException($"Part element is not available in {GetType()} template!");

        _rootButton.Click += OnRootButtonClick;
    }

    private void OnRootButtonClick(object sender, RoutedEventArgs e)
    {
        _rootPopup.IsOpen = true;
    }
}
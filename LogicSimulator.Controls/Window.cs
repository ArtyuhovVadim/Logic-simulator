using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace LogicSimulator.Controls;

[TemplatePart(Name = MinimizeButtonName, Type = typeof(Button))]
[TemplatePart(Name = MaximizeButtonName, Type = typeof(Button))]
[TemplatePart(Name = CloseButtonName, Type = typeof(Button))]
public class Window : System.Windows.Window
{
    private const string MinimizeButtonName = "PART_MinimizeButton";
    private const string MaximizeButtonName = "PART_MaximizeButton";
    private const string CloseButtonName = "PART_CloseButton";

    private Button _minimizeButton = null!;
    private Button _maximizeButton = null!;
    private Button _closeButton = null!;

    static Window()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _minimizeButton = GetTemplateChild(MinimizeButtonName) as Button ?? throw new ElementNotAvailableException($"Part element is not available in {GetType().Name} template!");
        _maximizeButton = GetTemplateChild(MaximizeButtonName) as Button ?? throw new ElementNotAvailableException($"Part element is not available in {GetType().Name} template!"); ;
        _closeButton = GetTemplateChild(CloseButtonName) as Button ?? throw new ElementNotAvailableException($"Part element is not available in {GetType().Name} template!"); ;

        _minimizeButton.Click += OnMinimizeButtonClicked;
        _maximizeButton.Click += OnMaximizeButtonClicked;
        _closeButton.Click += OnCloseButtonClicked;
    }

    private void OnMinimizeButtonClicked(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void OnMaximizeButtonClicked(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState switch
        {
            WindowState.Normal => WindowState.Maximized,
            WindowState.Maximized => WindowState.Normal,
            _ => WindowState
        };
    }

    private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
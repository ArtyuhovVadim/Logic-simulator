using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LogicSimulator.Controls;

public class TextBoxEx : TextBox
{
    #region IsHasError

    public bool IsHasError
    {
        get => (bool)GetValue(IsHasErrorProperty);
        set => SetValue(IsHasErrorProperty, value);
    }

    public static readonly DependencyProperty IsHasErrorProperty =
        DependencyProperty.Register(nameof(IsHasError), typeof(bool), typeof(TextBoxEx), new PropertyMetadata(false));

    #endregion

    #region SelectAllTextOnFocus

    public bool SelectAllTextOnFocus
    {
        get => (bool)GetValue(SelectAllTextOnFocusProperty);
        set => SetValue(SelectAllTextOnFocusProperty, value);
    }

    public static readonly DependencyProperty SelectAllTextOnFocusProperty =
        DependencyProperty.Register(nameof(SelectAllTextOnFocus), typeof(bool), typeof(TextBoxEx), new PropertyMetadata(false));

    #endregion

    private string _lastText = string.Empty;

    static TextBoxEx()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxEx), new FrameworkPropertyMetadata(typeof(TextBoxEx)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        GotFocus += OnGotFocus;
        KeyDown += OnKeyDown;
    }

    protected virtual void OnConfirm() { }

    protected virtual void OnReject() { }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                _lastText = Text;
                SelectAllText();
                OnConfirm();
                break;
            case Key.Escape:
                Text = _lastText;
                SelectAllText();
                OnReject();
                break;
        }
    }

    private void OnGotFocus(object sender, RoutedEventArgs e)
    {
        _lastText = Text;

        if (SelectAllTextOnFocus)
        {
            SelectAllText();
        }
    }

    private async void SelectAllText() => await Dispatcher.InvokeAsync(SelectAll);
}
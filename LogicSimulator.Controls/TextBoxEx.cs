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

    #region IsValueUndefined

    public bool IsValueUndefined
    {
        get => (bool)GetValue(IsValueUndefinedProperty);
        set => SetValue(IsValueUndefinedProperty, value);
    }

    public static readonly DependencyProperty IsValueUndefinedProperty =
        DependencyProperty.Register(nameof(IsValueUndefined), typeof(bool), typeof(TextBoxEx), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    #endregion

    #region ConfirmEvent

    public event RoutedEventHandler Confirm
    {
        add => AddHandler(ConfirmEvent, value);
        remove => RemoveHandler(ConfirmEvent, value);
    }

    public static readonly RoutedEvent ConfirmEvent =
        EventManager.RegisterRoutedEvent(nameof(Confirm), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxEx));

    #endregion

    #region RejectEvent

    public event RoutedEventHandler Reject
    {
        add => AddHandler(RejectEvent, value);
        remove => RemoveHandler(RejectEvent, value);
    }

    public static readonly RoutedEvent RejectEvent =
        EventManager.RegisterRoutedEvent(nameof(Reject), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxEx));

    #endregion

    private string _lastText = string.Empty;

    protected bool IsEnterPressed { get; private set; }
    protected bool IsTextChanged { get; private set; }

    static TextBoxEx()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxEx), new FrameworkPropertyMetadata(typeof(TextBoxEx)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        GotFocus += OnGotFocus;
        LostFocus += OnLostFocus;
        KeyDown += OnKeyDown;
        TextChanged += OnTextChanged;
    }

    protected virtual void OnConfirm() { }

    protected virtual void OnReject() { }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        IsTextChanged = true;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                OnConfirm();
                RaiseEvent(new RoutedEventArgs(ConfirmEvent, this));
                IsEnterPressed = true;
                _lastText = Text;
                SelectAllText();
                break;
            case Key.Escape:
                Text = _lastText;
                IsTextChanged = false;
                SelectAllText();
                OnReject();
                RaiseEvent(new RoutedEventArgs(RejectEvent, this));
                break;
        }
    }

    private void OnGotFocus(object sender, RoutedEventArgs e)
    {
        IsEnterPressed = false;
        IsTextChanged = false;
        _lastText = Text;

        if (SelectAllTextOnFocus)
        {
            SelectAllText();
        }
    }

    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
        _lastText = Text;
    }

    private async void SelectAllText() => await Dispatcher.InvokeAsync(SelectAll);
}
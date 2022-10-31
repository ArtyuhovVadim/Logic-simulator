using System.Windows;

namespace LogicSimulator.Controls;

public class StringBox : TextBoxEx
{
    #region String

    public string String
    {
        get => (string)GetValue(StringProperty);
        set => SetValue(StringProperty, value);
    }

    public static readonly DependencyProperty StringProperty =
        DependencyProperty.Register(nameof(String), typeof(string), typeof(StringBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStringChanged));

    private static void OnStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not StringBox stringBox) return;

        stringBox.Text = (string)e.NewValue;
    }

    #endregion

    static StringBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StringBox), new FrameworkPropertyMetadata(typeof(StringBox)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        Text = String;
    }

    protected override void OnConfirm()
    {
        IsValueUndefined = false;
        String = Text;
    }
}
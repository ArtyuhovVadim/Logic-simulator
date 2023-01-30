using System.Globalization;
using System.Windows;
using LogicSimulator.MathParser;

namespace LogicSimulator.Controls;

public class NumberBox : TextBoxEx
{
    #region Number

    public double Number
    {
        get => (double)GetValue(NumberProperty);
        set => SetValue(NumberProperty, value);
    }

    public static readonly DependencyProperty NumberProperty =
        DependencyProperty.Register(nameof(Number), typeof(double), typeof(NumberBox), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnNumberChanged));

    private static void OnNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NumberBox numberBox) return;

        var number = (double)e.NewValue;

        if (!numberBox.IsHasError)
        {
            numberBox.Text = numberBox.NumberToString(number);
            numberBox.RaiseEvent(new RoutedEventArgs(NumberChangedEvent, numberBox));
        }
    }

    #endregion

    #region DecimalPlaces

    public int DecimalPlaces
    {
        get => (int)GetValue(DecimalPlacesProperty);
        set => SetValue(DecimalPlacesProperty, value);
    }

    public static readonly DependencyProperty DecimalPlacesProperty =
        DependencyProperty.Register(nameof(DecimalPlaces), typeof(int), typeof(NumberBox), new PropertyMetadata(8), ValidateDecimalPlaces);

    private static bool ValidateDecimalPlaces(object value)
    {
        if (value is not int decimalPlaces) return false;

        if (decimalPlaces < 0) return false;

        return true;
    }

    #endregion

    #region MaxNumber

    public double MaxNumber
    {
        get => (double)GetValue(MaxNumberProperty);
        set => SetValue(MaxNumberProperty, value);
    }

    public static readonly DependencyProperty MaxNumberProperty =
        DependencyProperty.Register(nameof(MaxNumber), typeof(double), typeof(NumberBox), new PropertyMetadata(double.MaxValue, MaxNumberChanged));

    private static void MaxNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NumberBox numberBox) return;

        var maxNumber = (double)e.NewValue;

        if (maxNumber < numberBox.MinNumber)
            throw new ArgumentOutOfRangeException("MaxValue < MinValue");
    }

    #endregion

    #region MinNumber

    public double MinNumber
    {
        get => (double)GetValue(MinNumberProperty);
        set => SetValue(MinNumberProperty, value);
    }

    public static readonly DependencyProperty MinNumberProperty =
        DependencyProperty.Register(nameof(MinNumber), typeof(double), typeof(NumberBox), new PropertyMetadata(double.MinValue, MinNumberChanged));

    private static void MinNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NumberBox numberBox) return;

        var minNumber = (double)e.NewValue;

        if (minNumber > numberBox.MaxNumber)
            throw new ArgumentOutOfRangeException("MaxValue > MinValue");
    }

    #endregion

    #region RoundNumber

    public bool RoundNumber
    {
        get => (bool)GetValue(RoundNumberProperty);
        set => SetValue(RoundNumberProperty, value);
    }

    public static readonly DependencyProperty RoundNumberProperty =
        DependencyProperty.Register(nameof(RoundNumber), typeof(bool), typeof(NumberBox), new PropertyMetadata(false));

    #endregion

    #region NumberChangedEvent

    public event RoutedEventHandler NumberChanged
    {
        add => AddHandler(NumberChangedEvent, value);
        remove => RemoveHandler(NumberChangedEvent, value);
    }

    public static readonly RoutedEvent NumberChangedEvent =
        EventManager.RegisterRoutedEvent(nameof(NumberChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumberBox));

    #endregion

    private static readonly MathExpressionParser Parser = new();

    static NumberBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(typeof(NumberBox)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        Text = NumberToString(Number);
    }

    protected override void OnConfirm()
    {
        Number = ParseText(Text);

        if (!IsHasError)
        {
            Text = NumberToString(Number);
        }
    }

    private double ParseText(string text)
    {
        if (!Parser.TryParse(text, out var number) || number < MinNumber || number > MaxNumber)
        {
            IsHasError = true;
            return Number;
        }

        IsHasError = false;
        IsValueUndefined = false;

        return RoundNumber ? Math.Round(number, DecimalPlaces) : number;
    }

    private string NumberToString(double number) => number.ToString($"F{DecimalPlaces}", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
}
using System.Windows;
using System.Windows.Controls;
using LogicSimulator.Extensions;
using SharpDX;

namespace LogicSimulator.Controls;

[TemplatePart(Name = NumberBoxXName, Type = typeof(NumberBox))]
[TemplatePart(Name = NumberBoxYName, Type = typeof(NumberBox))]
public class Vector2Box : Control
{
    #region Vector

    public Vector2 Vector
    {
        get => (Vector2)GetValue(VectorProperty);
        set => SetValue(VectorProperty, value);
    }

    public static readonly DependencyProperty VectorProperty =
        DependencyProperty.Register(nameof(Vector), typeof(Vector2), typeof(Vector2Box), new FrameworkPropertyMetadata(Vector2.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnVectorChanged));

    private static void OnVectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Vector2Box vector2Box || vector2Box._numberBoxX is null || vector2Box._numberBoxY is null) return;

        var vector = (Vector2)e.NewValue;

        vector2Box._numberBoxX.Number = vector.X;
        vector2Box._numberBoxY.Number = vector.Y;
    }

    #endregion

    #region IsVectorXUndefined

    public bool IsVectorXUndefined
    {
        get => (bool)GetValue(IsVectorXUndefinedProperty);
        set => SetValue(IsVectorXUndefinedProperty, value);
    }

    public static readonly DependencyProperty IsVectorXUndefinedProperty =
        DependencyProperty.Register(nameof(IsVectorXUndefined), typeof(bool), typeof(Vector2Box), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnVectorXUndefinedChanged));

    private static void OnVectorXUndefinedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Vector2Box vector2Box || vector2Box._numberBoxX is null) return;

        vector2Box._numberBoxX.IsValueUndefined = (bool)e.NewValue;
    }

    #endregion

    #region IsVectorYUndefined

    public bool IsVectorYUndefined
    {
        get => (bool)GetValue(IsVectorYUndefinedProperty);
        set => SetValue(IsVectorYUndefinedProperty, value);
    }

    public static readonly DependencyProperty IsVectorYUndefinedProperty =
        DependencyProperty.Register(nameof(IsVectorYUndefined), typeof(bool), typeof(Vector2Box), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnVectorYUndefinedChanged));

    private static void OnVectorYUndefinedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Vector2Box vector2Box || vector2Box._numberBoxY is null) return;

        vector2Box._numberBoxY.IsValueUndefined = (bool)e.NewValue;
    }

    #endregion

    private const string NumberBoxXName = "PART_NumberBoxX";
    private const string NumberBoxYName = "PART_NumberBoxY";

    private NumberBox _numberBoxX;
    private NumberBox _numberBoxY;

    static Vector2Box()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Vector2Box), new FrameworkPropertyMetadata(typeof(Vector2Box)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _numberBoxX = this.GetTemplateChildOrThrowIfNull<NumberBox>(GetTemplateChild(NumberBoxXName));
        _numberBoxY = this.GetTemplateChildOrThrowIfNull<NumberBox>(GetTemplateChild(NumberBoxYName));

        _numberBoxX.Number = Vector.X;
        _numberBoxY.Number = Vector.Y;

        _numberBoxX.IsValueUndefined = IsVectorXUndefined;
        _numberBoxY.IsValueUndefined = IsVectorYUndefined;

        _numberBoxX.Confirm += OnXConfirm;
        _numberBoxY.Confirm += OnYConfirm;
    }

    private void OnXConfirm(object sender, RoutedEventArgs e)
    {
        IsVectorXUndefined = _numberBoxX.IsValueUndefined;
        Vector = new Vector2((float)_numberBoxX.Number, Vector.Y);
    }

    private void OnYConfirm(object sender, RoutedEventArgs e)
    {
        IsVectorYUndefined = _numberBoxY.IsValueUndefined;
        Vector = new Vector2(Vector.X, (float)_numberBoxY.Number);
    }
}
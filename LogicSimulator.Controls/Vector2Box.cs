using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
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

        _numberBoxX = GetTemplateChild(NumberBoxXName) as NumberBox;
        _numberBoxY = GetTemplateChild(NumberBoxYName) as NumberBox;

        if (_numberBoxX is null || _numberBoxY is null)
            throw new ElementNotAvailableException($"Part element is not available in {GetType()} template!");

        _numberBoxX.Number = Vector.X;
        _numberBoxY.Number = Vector.Y;

        _numberBoxX.NumberChanged += OnXNumberChanged;
        _numberBoxY.NumberChanged += OnYNumberChanged;
    }

    private void OnXNumberChanged(object sender, RoutedEventArgs e) => Vector = Vector with { X = (float)_numberBoxX.Number };

    private void OnYNumberChanged(object sender, RoutedEventArgs e) => Vector = Vector with { Y = (float)_numberBoxY.Number };
}
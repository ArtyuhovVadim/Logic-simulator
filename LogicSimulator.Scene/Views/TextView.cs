using System.Windows;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Color = System.Windows.Media.Color;
using Colors = System.Windows.Media.Colors;
using FontStretch = SharpDX.DirectWrite.FontStretch;
using FontStyle = SharpDX.DirectWrite.FontStyle;
using FontWeight = SharpDX.DirectWrite.FontWeight;

namespace LogicSimulator.Scene.Views;

public class TextView : SceneObjectView
{
    public static readonly IResource TextFormatResource = ResourceCache.Register<TextView>((factory, user) =>
        factory.CreateTextFormat(
            user.FontName,
            user.IsBold ? FontWeight.Bold : FontWeight.Normal,
            user.IsItalic ? FontStyle.Italic : FontStyle.Normal,
            FontStretch.Normal,
            user.FontSize));

    public static readonly IResource TextLayoutResource =
        ResourceCache.Register<TextView>((factory, user) => factory.CreateTextLayout(user.Text, user.Cache.Get<TextFormat>(user, TextFormatResource)));

    public static readonly IResource GeometryResource = ResourceCache.Register<TextView>((factory, user) =>
    {
        var textLayout = user.Cache.Get<TextLayout>(user, TextLayoutResource);

        var metrics = textLayout.Metrics;

        return factory.CreateRectangleGeometry(new RectangleF { Location = Vector2.Zero, Width = metrics.Width, Height = metrics.Height });
    });

    public static readonly IResource TextBrushResource =
        ResourceCache.Register<TextView>((factory, user) => factory.CreateSolidColorBrush(user.TextColor.ToColor4()));

    #region Text

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextView), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextPropertyChanged));

    private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextView textView) return;

        textView.ThrowIfDisposed();

        textView.Cache?.Update(textView, TextLayoutResource);
        textView.Cache?.Update(textView, GeometryResource);

        textView.MakeDirty();
    }

    #endregion

    #region FontName

    public string FontName
    {
        get => (string)GetValue(FontNameProperty);
        set => SetValue(FontNameProperty, value);
    }

    public static readonly DependencyProperty FontNameProperty =
        DependencyProperty.Register(nameof(FontName), typeof(string), typeof(TextView), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextLayoutChanged));

    #endregion

    #region FontSize

    public float FontSize
    {
        get => (float)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly DependencyProperty FontSizeProperty =
        DependencyProperty.Register(nameof(FontSize), typeof(float), typeof(TextView), new FrameworkPropertyMetadata(12f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextLayoutChanged));

    #endregion

    #region IsBold

    public bool IsBold
    {
        get => (bool)GetValue(IsBoldProperty);
        set => SetValue(IsBoldProperty, value);
    }

    public static readonly DependencyProperty IsBoldProperty =
        DependencyProperty.Register(nameof(IsBold), typeof(bool), typeof(TextView), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextLayoutChanged));

    #endregion

    #region IsItalic

    public bool IsItalic
    {
        get => (bool)GetValue(IsItalicProperty);
        set => SetValue(IsItalicProperty, value);
    }

    public static readonly DependencyProperty IsItalicProperty =
        DependencyProperty.Register(nameof(IsItalic), typeof(bool), typeof(TextView), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextLayoutChanged));

    #endregion

    #region IsUnderlined

    public bool IsUnderlined
    {
        get => (bool)GetValue(IsUnderlinedProperty);
        set => SetValue(IsUnderlinedProperty, value);
    }

    public static readonly DependencyProperty IsUnderlinedProperty =
        DependencyProperty.Register(nameof(IsUnderlined), typeof(bool), typeof(TextView), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region IsCross

    public bool IsCross
    {
        get => (bool)GetValue(IsCrossProperty);
        set => SetValue(IsCrossProperty, value);
    }

    public static readonly DependencyProperty IsCrossProperty =
        DependencyProperty.Register(nameof(IsCross), typeof(bool), typeof(TextView), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region TextColor

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly DependencyProperty TextColorProperty =
        DependencyProperty.Register(nameof(TextColor), typeof(Color), typeof(TextView), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextColorChanged));

    private static void OnTextColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextView textView) return;

        textView.ThrowIfDisposed();

        textView.Cache?.Update(textView, TextBrushResource);

        textView.MakeDirty();
    }

    #endregion

    public override bool HitTest(Vector2 pos, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<RectangleGeometry>(this, GeometryResource);
        return geometry.FillContainsPoint(pos, TransformMatrix * worldTransform, tolerance);
    }

    public override GeometryRelation HitTest(Geometry inputGeometry, Matrix3x2 worldTransform, float tolerance = 0.25f)
    {
        var geometry = Cache.Get<RectangleGeometry>(this, GeometryResource);
        return geometry.Compare(inputGeometry, Matrix3x2.Invert(TransformMatrix) * worldTransform, tolerance);
    }

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var textLayout = Cache.Get<TextLayout>(this, TextLayoutResource);
        var textBrush = Cache.Get<SolidColorBrush>(this, TextBrushResource);

        textLayout.SetUnderline(IsUnderlined, new TextRange(0, Text.Length));
        textLayout.SetStrikethrough(IsCross, new TextRange(0, Text.Length));

        context.DrawingContext.DrawTextLayout(Vector2.Zero, textLayout, textBrush, DrawTextOptions.None);
    }

    protected override void OnRenderSelection(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get<SolidColorBrush>(SelectionBrushStaticResource);
        var style = Cache.Get<StrokeStyle>(SelectionStyleStaticResource);
        var geometry = Cache.Get<RectangleGeometry>(this, GeometryResource);
        context.DrawingContext.DrawGeometry(geometry, brush, 1f / scene.Scale, style);
    }

    private static void OnTextLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextView textView) return;

        textView.ThrowIfDisposed();

        textView.Cache?.Update(textView, TextFormatResource);
        textView.Cache?.Update(textView, TextLayoutResource);
        textView.Cache?.Update(textView, GeometryResource);

        textView.MakeDirty();
    }
}
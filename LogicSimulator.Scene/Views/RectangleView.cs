using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using SharpDX;
using Color = System.Windows.Media.Color;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;

namespace LogicSimulator.Scene.Views;

public class RectangleView : SceneObjectView
{
    public static readonly IResource BackgroundBrushResource =
        ResourceCache.Register<RectangleView>((factory, user) => factory.CreateSolidColorBrush(ToColor4(user.Background)));

    #region Background

    public Color Background
    {
        get => (Color)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public static readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register(nameof(Background), typeof(Color), typeof(RectangleView), new PropertyMetadata(Colors.Black, OnBackgroundChanged));

    private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RectangleView rectangleView) return;

        rectangleView.Cache?.Update(rectangleView, BackgroundBrushResource);

        rectangleView.MakeDirty();
    }

    #endregion

    #region Width

    public float Width
    {
        get => (float)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register(nameof(Width), typeof(float), typeof(RectangleView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnWidthChanged));

    private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RectangleView rectangleView) return;

        rectangleView.MakeDirty();
    }

    #endregion

    #region Height

    public float Height
    {
        get => (float)GetValue(HeightProperty);
        set => SetValue(HeightProperty, value);
    }

    public static readonly DependencyProperty HeightProperty =
        DependencyProperty.Register(nameof(Height), typeof(float), typeof(RectangleView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHeightChanged));

    private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RectangleView rectangleView) return;

        rectangleView.MakeDirty();
    }

    #endregion

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var brush = Cache.Get<SolidColorBrush>(this, BackgroundBrushResource);
        context.DrawingContext.DrawRectangle(new RectangleF { Location = Location, Width = Width, Height = Height }, brush, 1f);
    }

    //TODO: Move
    private static Color4 ToColor4(Color color)
    {
        var a = color.A / 255f;
        var r = color.R / 255f;
        var g = color.G / 255f;
        var b = color.B / 255f;

        return new Color4(r, g, b, a);
    }
}
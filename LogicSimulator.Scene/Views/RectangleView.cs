using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using LogicSimulator.Utils;
using SharpDX;
using Color = System.Windows.Media.Color;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;

namespace LogicSimulator.Scene.Views;

public class RectangleView : SceneObjectView
{
    public static readonly IResource FillBrushResource =
        ResourceCache.Register<RectangleView>((factory, user) => factory.CreateSolidColorBrush(user.FillColor.ToColor4()));

    public static readonly IResource StrokeBrushResource =
        ResourceCache.Register<RectangleView>((factory, user) => factory.CreateSolidColorBrush(user.StrokeColor.ToColor4()));

    #region Width

    public float Width
    {
        get => (float)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register(nameof(Width), typeof(float), typeof(RectangleView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region Height

    public float Height
    {
        get => (float)GetValue(HeightProperty);
        set => SetValue(HeightProperty, value);
    }

    public static readonly DependencyProperty HeightProperty =
        DependencyProperty.Register(nameof(Height), typeof(float), typeof(RectangleView), new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region FillColor

    public Color FillColor
    {
        get => (Color)GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }

    public static readonly DependencyProperty FillColorProperty =
        DependencyProperty.Register(nameof(FillColor), typeof(Color), typeof(RectangleView), new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFillColorChanged));

    private static void OnFillColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RectangleView rectangleView) return;

        rectangleView.ThrowIfDisposed();

        rectangleView.Cache?.Update(rectangleView, FillBrushResource);

        rectangleView.MakeDirty();
    }

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(Color), typeof(RectangleView), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStrokeColorChanged));

    private static void OnStrokeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RectangleView rectangleView) return;

        rectangleView.ThrowIfDisposed();

        rectangleView.Cache?.Update(rectangleView, StrokeBrushResource);

        rectangleView.MakeDirty();
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => (float)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(float), typeof(RectangleView), new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    #region IsFilled

    public bool IsFilled
    {
        get => (bool)GetValue(IsFilledProperty);
        set => SetValue(IsFilledProperty, value);
    }

    public static readonly DependencyProperty IsFilledProperty =
        DependencyProperty.Register(nameof(IsFilled), typeof(bool), typeof(RectangleView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefaultPropertyChangedHandler));

    #endregion

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        if (IsFilled)
        {
            var fillBrush = Cache.Get<SolidColorBrush>(this, FillBrushResource);
            context.DrawingContext.FillRectangle(new RectangleF { Location = Location, Width = Width, Height = Height }, fillBrush);
        }

        var strokeBrush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);

        context.DrawingContext.DrawRectangle(new RectangleF { Location = Location, Width = Width, Height = Height }, strokeBrush, StrokeThickness);
    }
}
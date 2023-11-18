using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.Layers.Base;
using LogicSimulator.Scene.Layers.Renderers;
using LogicSimulator.Utils;

namespace LogicSimulator.Scene.Layers;

public class GridLayer : BaseSceneLayer
{
    public static readonly IResource BackgroundBrushResource =
        ResourceCache.Register<GridLayerRenderer>((factory, user) => factory.CreateSolidColorBrush(user.Layer.Background.ToColor4()));

    public static readonly IResource LineBrushResource =
        ResourceCache.Register<GridLayerRenderer>((factory, user) => factory.CreateSolidColorBrush(user.Layer.LineColor.ToColor4()));

    public static readonly IResource BoldLineBrushResource =
        ResourceCache.Register<GridLayerRenderer>((factory, user) => factory.CreateSolidColorBrush(user.Layer.BoldLineColor.ToColor4()));

    #region Background

    public Color Background
    {
        get => (Color)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public static readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register(nameof(Background), typeof(Color), typeof(GridLayer), new PropertyMetadata(Colors.White, OnBackgroundChanged));

    private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GridLayer gridLayer) return;
        
        gridLayer.Cache?.Update(gridLayer.Renderer, BackgroundBrushResource);

        gridLayer.MakeDirty();
    }

    #endregion

    #region LineColor

    public Color LineColor
    {
        get => (Color)GetValue(LineColorProperty);
        set => SetValue(LineColorProperty, value);
    }

    public static readonly DependencyProperty LineColorProperty =
        DependencyProperty.Register(nameof(LineColor), typeof(Color), typeof(GridLayer), new PropertyMetadata(Colors.Black, OnColorChanged));

    private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GridLayer gridLayer) return;

        gridLayer.Cache?.Update(gridLayer.Renderer, LineBrushResource);

        gridLayer.MakeDirty();
    }

    #endregion

    #region BoldLineColor

    public Color BoldLineColor
    {
        get => (Color)GetValue(BoldLineColorProperty);
        set => SetValue(BoldLineColorProperty, value);
    }

    public static readonly DependencyProperty BoldLineColorProperty =
        DependencyProperty.Register(nameof(BoldLineColor), typeof(Color), typeof(GridLayer), new PropertyMetadata(Colors.Black, OnBoldLineColorChanged));

    private static void OnBoldLineColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GridLayer gridLayer) return;

        gridLayer.Cache?.Update(gridLayer.Renderer, BoldLineBrushResource);

        gridLayer.MakeDirty();
    }

    #endregion

    #region LineThickness

    public float LineThickness
    {
        get => (float)GetValue(LineThicknessProperty);
        set => SetValue(LineThicknessProperty, value);
    }

    public static readonly DependencyProperty LineThicknessProperty =
        DependencyProperty.Register(nameof(LineThickness), typeof(float), typeof(GridLayer), new PropertyMetadata(1f, DefaultPropertyChangedHandler));

    #endregion

    #region Width

    public int Width
    {
        get => (int)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register(nameof(Width), typeof(int), typeof(GridLayer), new PropertyMetadata(300, DefaultPropertyChangedHandler));

    #endregion

    #region Height

    public int Height
    {
        get => (int)GetValue(HeightProperty);
        set => SetValue(HeightProperty, value);
    }

    public static readonly DependencyProperty HeightProperty =
        DependencyProperty.Register(nameof(Height), typeof(int), typeof(GridLayer), new PropertyMetadata(300, DefaultPropertyChangedHandler));

    #endregion

    #region CellSize

    public int CellSize
    {
        get => (int)GetValue(CellSizeProperty);
        set => SetValue(CellSizeProperty, value);
    }

    public static readonly DependencyProperty CellSizeProperty =
        DependencyProperty.Register(nameof(CellSize), typeof(int), typeof(GridLayer), new PropertyMetadata(25, DefaultPropertyChangedHandler));

    #endregion

    #region BoldLineStep

    public int BoldLineStep
    {
        get => (int)GetValue(BoldLineStepProperty);
        set => SetValue(BoldLineStepProperty, value);
    }

    public static readonly DependencyProperty BoldLineStepProperty =
        DependencyProperty.Register(nameof(BoldLineStep), typeof(int), typeof(GridLayer), new PropertyMetadata(10, DefaultPropertyChangedHandler));

    #endregion
}
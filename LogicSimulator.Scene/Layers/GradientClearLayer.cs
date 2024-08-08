using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.Layers.Base;
using LogicSimulator.Scene.Layers.Renderers;
using LogicSimulator.Shared.ExtensionMethods;
using SharpDX;
using SharpDX.Direct2D1;
using Color = System.Windows.Media.Color;
using GradientStop = SharpDX.Direct2D1.GradientStop;
using LinearGradientBrush = SharpDX.Direct2D1.LinearGradientBrush;

namespace LogicSimulator.Scene.Layers;

public class GradientClearLayer : BaseSceneLayer
{
    #region PixelSize

    public Size2F PixelSize
    {
        get => (Size2F)GetValue(PixelSizeProperty);
        set => SetValue(PixelSizeProperty, value);
    }

    public static readonly DependencyProperty PixelSizeProperty =
        DependencyProperty.Register(nameof(PixelSize), typeof(Size2F), typeof(GradientClearLayer), new PropertyMetadata(default(Size2F), OnPixelSizePropertyChanged));

    private static void OnPixelSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GradientClearLayer layer) return;

        layer.ThrowIfDisposed();

        layer.Cache?.Update((GradientClearRenderer)layer.Renderer, BrushResource);

        layer.MakeDirty();
    }

    #endregion

    public static readonly IResource<GradientClearRenderer, LinearGradientBrush> BrushResource = ResourceCache.Register<GradientClearRenderer, LinearGradientBrush>((factory, user) =>
    {
        var width = user.Layer.PixelSize.Width;
        var height = user.Layer.PixelSize.Height;

        var gradientStopCollection = new GradientStop[]
        {
            new() { Position = 0f, Color = user.Layer.StartColor.ToColor4() },
            new() { Position = 1f, Color = user.Layer.EndColor.ToColor4() }
        };

        var properties = new LinearGradientBrushProperties
        {
            StartPoint = new Vector2(width / 2, 0),
            EndPoint = new Vector2(width / 2, height)
        };

        return factory.CreateLinearGradientBrush(properties, gradientStopCollection);
    });

    #region StartColor

    public Color StartColor
    {
        get => (Color)GetValue(StartColorProperty);
        set => SetValue(StartColorProperty, value);
    }

    public static readonly DependencyProperty StartColorProperty =
        DependencyProperty.Register(nameof(StartColor), typeof(Color), typeof(GradientClearLayer), new PropertyMetadata(Colors.White, OnColorChanged));

    #endregion

    #region EndColor

    public Color EndColor
    {
        get => (Color)GetValue(EndColorProperty);
        set => SetValue(EndColorProperty, value);
    }

    public static readonly DependencyProperty EndColorProperty =
        DependencyProperty.Register(nameof(EndColor), typeof(Color), typeof(GradientClearLayer), new PropertyMetadata(Colors.Black, OnColorChanged));

    #endregion

    private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GradientClearLayer layer) return;

        layer.ThrowIfDisposed();

        layer.Cache?.Update((GradientClearRenderer)layer.Renderer, BrushResource);

        layer.MakeDirty();
    }
}
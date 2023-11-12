using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.Cache;
using LogicSimulator.Scene.Layers.Base;
using LogicSimulator.Scene.Layers.Renderers;
using LogicSimulator.Utils;
using SharpDX;
using SharpDX.Direct2D1;
using Color = System.Windows.Media.Color;
using GradientStop = SharpDX.Direct2D1.GradientStop;

namespace LogicSimulator.Scene.Layers;

public class GradientClearLayer : BaseSceneLayer
{
    public static readonly IResource BrushResource = ResourceCache.Register<GradientClearRenderer>((factory, user) =>
    {
        //TODO: Костыль, можно передавать width и height через свойства зависимости
        var scene = (Scene2D)user.Layer.Parent;

        var width = scene.PixelSize.Width;
        var height = scene.PixelSize.Height;

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
        DependencyProperty.Register(nameof(StartColor), typeof(Color), typeof(GradientClearLayer), new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));

    #endregion

    #region EndColor

    public Color EndColor
    {
        get => (Color)GetValue(EndColorProperty);
        set => SetValue(EndColorProperty, value);
    }

    public static readonly DependencyProperty EndColorProperty =
        DependencyProperty.Register(nameof(EndColor), typeof(Color), typeof(GradientClearLayer), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));

    private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GradientClearLayer layer) return;

        layer.ThrowIfDisposed();

        layer.Cache?.Update(layer.Renderer, BrushResource);

        layer.MakeDirty();
    }

    #endregion
}
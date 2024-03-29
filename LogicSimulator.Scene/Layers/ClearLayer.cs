﻿using System.Windows;
using System.Windows.Media;
using LogicSimulator.Scene.Layers.Base;

namespace LogicSimulator.Scene.Layers;

public class ClearLayer : BaseSceneLayer
{
    #region Color

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ClearLayer), new PropertyMetadata(default(Color), DefaultPropertyChangedHandler));

    #endregion
}
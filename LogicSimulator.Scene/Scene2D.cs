﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.ExtensionMethods;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools;
using LogicSimulator.Scene.Tools.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene;

public class Scene2D : FrameworkElement
{
    private readonly SceneRenderer _renderer;

    private SceneTransformController _sceneTransformController;

    private bool _isLeftMouseButtonPressedOnScene;

    #region Objects

    public ObservableCollection<BaseSceneObject> Objects
    {
        get => (ObservableCollection<BaseSceneObject>)GetValue(ObjectsProperty);
        set => SetValue(ObjectsProperty, value);
    }

    public static readonly DependencyProperty ObjectsProperty =
        DependencyProperty.Register(nameof(Objects), typeof(ObservableCollection<BaseSceneObject>), typeof(Scene2D),
            new PropertyMetadata(null));

    #endregion

    #region Components

    public ObservableCollection<BaseRenderingComponent> Components
    {
        get => (ObservableCollection<BaseRenderingComponent>)GetValue(ComponentsProperty);
        set => SetValue(ComponentsProperty, value);
    }

    public static readonly DependencyProperty ComponentsProperty =
        DependencyProperty.Register(nameof(Components), typeof(ObservableCollection<BaseRenderingComponent>), typeof(Scene2D),
            new PropertyMetadata(null));

    #endregion

    #region Tools

    public IEnumerable<BaseTool> Tools
    {
        get => (IEnumerable<BaseTool>)GetValue(ToolsProperty);
        set => SetValue(ToolsProperty, value);
    }

    public static readonly DependencyProperty ToolsProperty =
        DependencyProperty.Register(nameof(Tools), typeof(IEnumerable<BaseTool>), typeof(Scene2D),
            new PropertyMetadata(Enumerable.Empty<BaseTool>(), OnToolsChanged));

    private static void OnToolsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene) return;

        scene.CurrentTool = ((IEnumerable<BaseTool>)e.NewValue).First();
    }

    #endregion

    #region Scale

    public float Scale
    {
        get => (float)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    public static readonly DependencyProperty ScaleProperty =
        DependencyProperty.Register(nameof(Scale), typeof(float), typeof(Scene2D), new PropertyMetadata(1f, OnScaleChanged));

    private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Scene2D scene) return;

        var newValue = (float)e.NewValue;
        var renderer = scene._renderer;

        renderer.Transform = renderer.Transform with { M11 = newValue, M22 = newValue };
        renderer.RequestRender();
    }

    #endregion

    #region MousePosition

    private static readonly DependencyPropertyKey MousePositionPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(MousePosition), typeof(Vector2), typeof(Scene2D), new PropertyMetadata(Vector2.Zero));

    public static readonly DependencyProperty MousePositionProperty
        = MousePositionPropertyKey.DependencyProperty;

    public Vector2 MousePosition
    {
        get => (Vector2)GetValue(MousePositionProperty);
        private set => SetValue(MousePositionPropertyKey, value);
    }

    #endregion

    public float Dpi => (float)VisualTreeHelper.GetDpi(this).PixelsPerInchX;

    //TODO: Костыль!
    internal RenderTarget RenderTarget => _renderer.RenderTarget;

    public Matrix3x2 Transform => _renderer.Transform;

    public Vector2 Translation
    {
        get => new(_renderer.Transform.M31, _renderer.Transform.M32);
        set
        {
            _renderer.Transform = _renderer.Transform with { M31 = value.X, M32 = value.Y };
            _renderer.RequestRender();
        }
    }

    public Scene2D()
    {
        ClipToBounds = true;
        Focusable = true;
        VisualEdgeMode = EdgeMode.Aliased;

        _sceneTransformController = new SceneTransformController(this);

        _renderer = new SceneRenderer(this);

        Loaded += OnLoaded;

        CompositionTarget.Rendering += (_, _) => _renderer.RequestRender();
    }

    public BaseTool CurrentTool { get; private set; }

    public T GetComponent<T>() where T : BaseRenderingComponent
    {
        return (T)Components.FirstOrDefault(x => x.GetType() == typeof(T));
    }

    public T GetTool<T>() where T : BaseTool
    {
        return (T)Tools.FirstOrDefault(x => x.GetType() == typeof(T));
    }

    public void SwitchTool<T>() where T : BaseTool
    {
        if (Tools is null)
            throw new ApplicationException("There are no tools in the scene!");

        var tool = Tools.FirstOrDefault(x => x.GetType() == typeof(T));

        if (tool is null) return;

        if (CurrentTool is null)
        {
            CurrentTool = tool;
            return;
        }

        CurrentTool?.Deactivate(this);

        CurrentTool = tool;

        CurrentTool.Activate(this);
    }

    protected override void OnRender(DrawingContext drawingContext) => _renderer.WpfRender(drawingContext);

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) => _renderer.Resize(sizeInfo.NewSize);

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        var pos = GetMousePosition();

        MousePosition = pos.Transform(Transform);

        if (e.LeftButton == MouseButtonState.Pressed && _isLeftMouseButtonPressedOnScene)
            CurrentTool?.MouseLeftButtonDragged(this, pos);
        else if (e.RightButton == MouseButtonState.Pressed && _isLeftMouseButtonPressedOnScene)
            CurrentTool?.MouseRightButtonDragged(this, pos);
        else if (e.MiddleButton == MouseButtonState.Pressed && _isLeftMouseButtonPressedOnScene)
            CurrentTool?.MouseMiddleButtonDragged(this, pos);
        else
            CurrentTool?.MouseMove(this, pos);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        _isLeftMouseButtonPressedOnScene = true;

        CurrentTool?.MouseLeftButtonDown(this, GetMousePosition());
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        if (_isLeftMouseButtonPressedOnScene)
            CurrentTool?.MouseLeftButtonUp(this, GetMousePosition());

        _isLeftMouseButtonPressedOnScene = false;

        Mouse.Capture(null);
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonDown(e);

        CurrentTool?.MouseRightButtonDown(this, GetMousePosition());
    }

    protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonUp(e);

        CurrentTool?.MouseRightButtonUp(this, GetMousePosition());

        Mouse.Capture(null);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);

        Mouse.Capture(this);
        Keyboard.Focus(this);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);

        if (e.ChangedButton == MouseButton.Middle)
        {
            Mouse.Capture(null);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        CurrentTool?.KeyDown(this, e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        CurrentTool?.KeyUp(this, e);
    }

    protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        base.OnLostKeyboardFocus(e);

        SwitchTool<SelectionTool>();
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => _renderer.SetOwnerWindow();

    private Vector2 GetMousePosition() => Mouse.GetPosition(this).ToVector2().DpiCorrect(Dpi);
}
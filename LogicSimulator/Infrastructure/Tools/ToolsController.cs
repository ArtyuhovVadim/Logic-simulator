using System.Collections.Specialized;
using System.Windows;
using System.Windows.Markup;
using LogicSimulator.Infrastructure.Tools.Base;

namespace LogicSimulator.Infrastructure.Tools;

public class ToolsCollection : FreezableCollection<BaseTool> { }

[ContentProperty(nameof(Tools))]
public class ToolsController : Freezable
{
    private BaseTool _currentTool;

    public event Action<BaseTool, BaseTool> ToolChanged;

    #region Controller

    public static readonly DependencyProperty ControllerProperty = DependencyProperty.RegisterAttached(
        "ShadowController", typeof(ToolsController), typeof(ToolsController), new PropertyMetadata(default(ToolsController)));

    public static void SetController(DependencyObject element, ToolsController value) => element.SetValue(ControllerProperty, value);

    public static ToolsController GetController(DependencyObject element) => (ToolsController)element.GetValue(ControllerProperty);

    #endregion

    #region Tools

    private static readonly DependencyPropertyKey ToolsPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(Tools), typeof(ToolsCollection), typeof(ToolsController), new PropertyMetadata(default(ToolsCollection)));

    public ToolsCollection Tools
    {
        get => (ToolsCollection)GetValue(ToolsPropertyKey.DependencyProperty);
        private set => SetValue(ToolsPropertyKey, value);
    }

    #endregion

    #region CurrentToolContext

    public object CurrentToolContext
    {
        get => GetValue(CurrentToolContextProperty);
        set => SetValue(CurrentToolContextProperty, value);
    }

    public static readonly DependencyProperty CurrentToolContextProperty =
        DependencyProperty.Register(nameof(CurrentToolContext), typeof(object), typeof(ToolsController), new PropertyMetadata(default, OnCurrentToolContextPropertyChanged));

    private static void OnCurrentToolContextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ToolsController controller) return;

        var tool = controller.Tools.FirstOrDefault(x => x.Context == e.NewValue);

        if (tool is null) return;

        controller.CurrentTool = tool;
    }

    #endregion

    #region DefaultToolContext

    public object DefaultToolContext
    {
        get => GetValue(DefaultToolContextProperty);
        set => SetValue(DefaultToolContextProperty, value);
    }

    public static readonly DependencyProperty DefaultToolContextProperty =
        DependencyProperty.Register(nameof(DefaultToolContext), typeof(object), typeof(ToolsController), new PropertyMetadata(default(object)));

    #endregion

    public ToolsController()
    {
        Tools = new ToolsCollection();

        ((INotifyCollectionChanged)Tools).CollectionChanged += OnToolCollectionChanged;
    }

    public BaseTool CurrentTool
    {
        get => _currentTool;
        set
        {
            if (value == _currentTool) return;

            OnToolChanged(value, _currentTool, false);
        }
    }

    public void SwitchTool<T>(Action<T>? actionToNextToolAfterActivating = null) where T : BaseTool
    {
        var nextTool = Tools.FirstOrDefault(x => x.GetType() == typeof(T));

        if (nextTool is null) return;

        if (nextTool == _currentTool) return;

        OnToolChanged(nextTool, _currentTool, true);

        actionToNextToolAfterActivating?.Invoke((T)nextTool);
    }

    public void SwitchToDefaultTool()
    {
        var defaultTool = Tools.FirstOrDefault(x => x.Context == DefaultToolContext);

        OnToolChanged(defaultTool, _currentTool, false);
    }

    protected override Freezable CreateInstanceCore() => new ToolsController();

    private void OnToolCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems?.Count == 0)
            return;

        foreach (var tool in e.NewItems!)
        {
            if (Tools.Count(x => x.GetType() == tool.GetType()) > 1)
            {
                throw new InvalidOperationException($"{tool.GetType().Name} has already been added.");
            }
        }

        foreach (BaseTool newTool in e.NewItems!)
        {
            if (newTool.Context is null)
                newTool.ContextChanged += OnContextChanged;
        }
    }

    private void OnContextChanged(BaseTool tool)
    {
        tool.ContextChanged -= OnContextChanged;

        var newTool = Tools.FirstOrDefault(x => x.Context == CurrentToolContext);

        if (newTool is null) return;

        CurrentTool = newTool;
    }

    private void OnToolChanged(BaseTool newTool, BaseTool oldTool, bool activatedFromOtherTool)
    {
        if (oldTool is { CanSwitch: false })
            return;

        oldTool?.Deactivate();
        newTool?.Activate(this, activatedFromOtherTool);

        _currentTool = newTool;
        CurrentToolContext = newTool?.Context;

        ToolChanged?.Invoke(newTool, oldTool);
    }
}
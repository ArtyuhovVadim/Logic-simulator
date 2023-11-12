using System.Collections.Specialized;
using System.Windows;
using System.Windows.Markup;
using LogicSimulator.Infrastructure.Tools.Base;
using LogicSimulator.Scene;

namespace LogicSimulator.Infrastructure.Tools;

[ContentProperty(nameof(Tools))]
public class ToolsController : Freezable
{
    public event Action<BaseTool, BaseTool> ToolChanged;

    #region ToolsController

    public static readonly DependencyProperty ToolsControllerProperty = DependencyProperty.RegisterAttached(
        "ToolsController", typeof(ToolsController), typeof(Scene2D), new PropertyMetadata(default(ToolsController)));

    public static void SetToolsController(Scene2D scene, ToolsController value) => scene.SetValue(ToolsControllerProperty, value);

    public static ToolsController GetToolsController(Scene2D scene) => (ToolsController)scene.GetValue(ToolsControllerProperty);

    #endregion

    #region CurrentTool

    public BaseTool CurrentTool
    {
        get => (BaseTool)GetValue(CurrentToolProperty);
        set => SetValue(CurrentToolProperty, value);
    }

    public static readonly DependencyProperty CurrentToolProperty =
        DependencyProperty.Register(nameof(CurrentTool), typeof(BaseTool), typeof(ToolsController), new PropertyMetadata(default(BaseTool), OnCurrentToolPropertyChanged, OnCoerceCurrentToolProperty));

    private static object OnCoerceCurrentToolProperty(DependencyObject d, object baseValue)
    {
        if (d is not ToolsController controller)
            return DependencyProperty.UnsetValue;

        if (controller.CurrentTool is null)
            return baseValue;

        return !controller.CurrentTool.CanSwitch ? controller.CurrentTool : baseValue;
    }

    private static void OnCurrentToolPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ToolsController controller) return;

        var newTool = (BaseTool)e.NewValue;
        var oldTool = (BaseTool)e.OldValue;

        oldTool?.Deactivate();
        newTool?.Activate(controller);

        controller.ToolChanged?.Invoke(newTool, oldTool);
    }

    #endregion

    #region Tools

    private static readonly DependencyPropertyKey ToolsPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(Tools), typeof(FreezableCollection<BaseTool>), typeof(ToolsController), new FrameworkPropertyMetadata(default(FreezableCollection<BaseTool>)));

    public FreezableCollection<BaseTool> Tools => (FreezableCollection<BaseTool>)GetValue(ToolsPropertyKey.DependencyProperty);

    #endregion

    public ToolsController()
    {
        SetValue(ToolsPropertyKey, new FreezableCollection<BaseTool>());
        ((INotifyCollectionChanged)Tools).CollectionChanged += OnToolsCollectionChanged;
    }

    public void SwitchTool<T>(Action<T>? actionToNextToolAfterActivating = null) where T : BaseTool
    {
        var nextTool = Tools.FirstOrDefault(x => x.GetType() == typeof(T));

        if (nextTool is null) return;

        CurrentTool = nextTool;
        actionToNextToolAfterActivating?.Invoke((T)nextTool);
    }

    protected override Freezable CreateInstanceCore() => new ToolsController();

    private void OnToolsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is not (NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Replace)) return;

        foreach (var newTool in e.NewItems!.Cast<BaseTool>())
        {
            if (Tools.Count(x => x.GetType() == newTool.GetType()) > 1)
            {
                throw new InvalidOperationException($"{newTool.GetType().Name} has already been added.");
            }
        }
    }
}
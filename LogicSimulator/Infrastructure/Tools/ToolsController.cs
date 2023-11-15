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
            var newTool = value;
            var oldTool = _currentTool;

            if (oldTool is { CanSwitch: false })
                return;

            oldTool?.Deactivate();
            newTool?.Activate(this);

            _currentTool = value;

            ToolChanged?.Invoke(newTool, oldTool);
        }
    }

    public void SwitchTool<T>(Action<T>? actionToNextToolAfterActivating = null) where T : BaseTool
    {
        var nextTool = Tools.FirstOrDefault(x => x.GetType() == typeof(T));

        if (nextTool is null) return;

        CurrentTool = nextTool;
        actionToNextToolAfterActivating?.Invoke((T)nextTool);
    }

    protected override Freezable CreateInstanceCore() => new ToolsController();

    private void OnToolCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        CurrentTool ??= Tools.FirstOrDefault();
    }
}
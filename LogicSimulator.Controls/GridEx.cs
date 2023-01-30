using System.Windows.Controls;
using System.Windows;

namespace LogicSimulator.Controls;

public class GridEx : Grid
{
    #region ColumnsLayout

    public IEnumerable<GridLength> ColumnsLayout
    {
        get => (IEnumerable<GridLength>)GetValue(ColumnsLayoutProperty);
        set => SetValue(ColumnsLayoutProperty, value);
    }

    public static readonly DependencyProperty ColumnsLayoutProperty =
        DependencyProperty.Register(nameof(ColumnsLayout), typeof(IEnumerable<GridLength>), typeof(GridEx), new PropertyMetadata(null, OnColumnsLayoutChanged));

    private static void OnColumnsLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GridEx grid) return;

        foreach (var length in (IEnumerable<GridLength>)e.NewValue)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = length });
        }
    }

    #endregion

    protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
    {
        base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        if (visualAdded is null) return;
        if (ColumnsLayout.Count() < Children.Count) return;
        SetColumn(Children[^1], Children.Count - 1);
    }
}

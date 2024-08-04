using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace LogicSimulator.Infrastructure.Behaviors;

public class DataGridScrollToLastItemBehaviour : Behavior<DataGrid>
{
    protected override void OnAttached()
    {
        if (!AssociatedObject.IsLoaded)
        {
            AssociatedObject.Loaded += OnLoaded;
            return;
        }

        if (AssociatedObject.ItemsSource is not INotifyCollectionChanged collection) return;

        collection.CollectionChanged += OnCollectionChanged;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject.ItemsSource is not INotifyCollectionChanged collection) return;

        collection.CollectionChanged -= OnCollectionChanged;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            AssociatedObject.ScrollIntoView(AssociatedObject.ItemsSource.Cast<object>().Last());
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (AssociatedObject.ItemsSource is not INotifyCollectionChanged collection) return;

        collection.CollectionChanged += OnCollectionChanged;
        AssociatedObject.Loaded -= OnLoaded;
    }
}
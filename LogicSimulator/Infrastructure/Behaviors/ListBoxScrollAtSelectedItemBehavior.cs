using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace LogicSimulator.Infrastructure.Behaviors;

public class ListBoxScrollAtSelectedItemBehavior : Behavior<ListBox>
{
    protected override void OnAttached()
    {
        AssociatedObject.SelectionChanged += OnListBoxSelectionChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.SelectionChanged -= OnListBoxSelectionChanged;
    }

    private void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        AssociatedObject.ScrollIntoView(AssociatedObject.SelectedItem);
    }
}
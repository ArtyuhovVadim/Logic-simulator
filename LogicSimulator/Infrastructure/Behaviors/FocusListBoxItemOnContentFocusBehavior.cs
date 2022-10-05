using System.Windows;
using System.Windows.Controls;
using LogicSimulator.ViewModels.Base;
using Microsoft.Xaml.Behaviors;

namespace LogicSimulator.Infrastructure.Behaviors;

public class FocusListBoxItemOnContentFocusBehavior : Behavior<Control>
{
    #region ListBox

    public ListBox ListBox
    {
        get => (ListBox)GetValue(ListBoxProperty);
        set => SetValue(ListBoxProperty, value);
    }

    public static readonly DependencyProperty ListBoxProperty =
        DependencyProperty.Register(nameof(ListBox), typeof(ListBox), typeof(FocusListBoxItemOnContentFocusBehavior), new PropertyMetadata(default(ListBox)));

    #endregion

    #region ViewModel

    public BindableBase ViewModel
    {
        get => (BindableBase)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(BindableBase), typeof(FocusListBoxItemOnContentFocusBehavior), new PropertyMetadata(default(BindableBase)));

    #endregion

    protected override void OnAttached()
    {
        AssociatedObject.GotFocus += OnGotFocus;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.GotFocus -= OnGotFocus;
    }

    private void OnGotFocus(object sender, RoutedEventArgs e)
    {
        if (ListBox.ItemContainerGenerator.ContainerFromItem(ViewModel) is ListBoxItem listBoxItem)
        {
            listBoxItem.IsSelected = true;
        }
    }
}
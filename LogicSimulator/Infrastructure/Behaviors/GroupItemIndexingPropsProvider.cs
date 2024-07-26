using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;
using WpfExtensions.Utils;

namespace LogicSimulator.Infrastructure.Behaviors;

public class GroupItemIndexingPropsProvider : Behavior<FrameworkElement>
{
    #region IsFirstGroup

    private static readonly DependencyPropertyKey IsFirstGroupPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(IsFirstGroup), typeof(bool), typeof(GroupItemIndexingPropsProvider), new PropertyMetadata(default(bool)));

    public static readonly DependencyProperty IsFirstGroupProperty = IsFirstGroupPropertyKey.DependencyProperty;

    public bool IsFirstGroup
    {
        get => (bool)GetValue(IsFirstGroupProperty);
        private set => SetValue(IsFirstGroupPropertyKey, value);
    }

    #endregion

    #region IsLastGroup

    private static readonly DependencyPropertyKey IsLastGroupPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(IsLastGroup), typeof(bool), typeof(GroupItemIndexingPropsProvider), new PropertyMetadata(default(bool)));

    public static readonly DependencyProperty IsLastGroupProperty = IsLastGroupPropertyKey.DependencyProperty;

    public bool IsLastGroup
    {
        get => (bool)GetValue(IsLastGroupProperty);
        private set => SetValue(IsLastGroupPropertyKey, value);
    }

    #endregion

    #region GroupIndex

    private static readonly DependencyPropertyKey GroupIndexPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(GroupIndex), typeof(int), typeof(GroupItemIndexingPropsProvider), new PropertyMetadata(default(int)));

    public static readonly DependencyProperty GroupIndexProperty = GroupIndexPropertyKey.DependencyProperty;

    public int GroupIndex
    {
        get => (int)GetValue(GroupIndexProperty);
        private set => SetValue(GroupIndexPropertyKey, value);
    }

    #endregion

    #region GroupsCount

    private static readonly DependencyPropertyKey GroupsCountPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(GroupsCount), typeof(int), typeof(GroupItemIndexingPropsProvider), new PropertyMetadata(default(int)));

    public static readonly DependencyProperty GroupsCountProperty = GroupsCountPropertyKey.DependencyProperty;

    public int GroupsCount
    {
        get => (int)GetValue(GroupsCountProperty);
        private set => SetValue(GroupsCountPropertyKey, value);
    }

    #endregion

    protected override void OnAttached()
    {
        var parent = AssociatedObject.FindVisualParent<ItemsControl>();
        var groupItem = AssociatedObject.FindVisualParent<GroupItem>();

        if (groupItem is null)
            throw new InvalidOperationException("GroupItem not found.");

        if (parent is null)
            throw new InvalidOperationException("Parent ItemsControl not found.");

        if (!parent.IsGrouping)
            throw new InvalidOperationException("Parent ItemsControl isn't grouping.");

        var groupItemCollectionViewGroup = (CollectionViewGroup)groupItem.DataContext;
        var parentCollectionViewGroups = parent.ItemContainerGenerator.Items.Cast<CollectionViewGroup>().ToList();

        var index = parentCollectionViewGroups.IndexOf(groupItemCollectionViewGroup);

        GroupsCount = parentCollectionViewGroups.Count;
        GroupIndex = index;
        IsLastGroup = index == parentCollectionViewGroups.Count - 1;
        IsFirstGroup = index == 0;
    }
}
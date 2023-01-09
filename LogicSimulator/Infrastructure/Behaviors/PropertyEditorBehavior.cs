using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;
using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.Infrastructure.Behaviors;

public class UndefinedProperty
{
    public DependencyProperty DependencyProperty { get; set; }

    public string Suffix { get; set; } = string.Empty;
}

public class PropertyEditorBehavior : Behavior<FrameworkElement>
{
    #region EditorDependencyProperty

    public DependencyProperty EditorDependencyProperty
    {
        get => (DependencyProperty)GetValue(EditorDependencyPropertyProperty);
        set => SetValue(EditorDependencyPropertyProperty, value);
    }

    public static readonly DependencyProperty EditorDependencyPropertyProperty =
        DependencyProperty.Register(nameof(EditorDependencyProperty), typeof(DependencyProperty), typeof(PropertyEditorBehavior), new PropertyMetadata(default(DependencyProperty)));

    #endregion

    #region ObjectPropertyName

    public string ObjectPropertyName
    {
        get => (string)GetValue(ObjectPropertyNameProperty);
        set => SetValue(ObjectPropertyNameProperty, value);
    }

    public static readonly DependencyProperty ObjectPropertyNameProperty =
        DependencyProperty.Register(nameof(ObjectPropertyName), typeof(string), typeof(PropertyEditorBehavior), new PropertyMetadata(default(string)));

    #endregion

    #region EditorViewModel

    public AbstractEditorViewModel EditorViewModel
    {
        get => (AbstractEditorViewModel)GetValue(EditorViewModelProperty);
        set => SetValue(EditorViewModelProperty, value);
    }

    public static readonly DependencyProperty EditorViewModelProperty =
        DependencyProperty.Register(nameof(EditorViewModel), typeof(AbstractEditorViewModel), typeof(PropertyEditorBehavior), new PropertyMetadata(default(AbstractEditorViewModel), OnEditorViewModelChanged));

    private static void OnEditorViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PropertyEditorBehavior behavior) return;

        if (e.NewValue != e.OldValue)
        {
            behavior.SetBindings();
        }
    }

    #endregion

    public Collection<UndefinedProperty> UndefinedProperties { get; set; } = new();

    private void SetBindings()
    {
        if (ObjectPropertyName is null) return;

        var propertyBinding = new Binding(ObjectPropertyName) { Source = EditorViewModel };
        AssociatedObject.SetBinding(EditorDependencyProperty, propertyBinding);

        foreach (var undefinedProperty in UndefinedProperties)
        {
            var undefinedMapBinding = new Binding($"UndefinedPropertiesMap[{ObjectPropertyName}{undefinedProperty.Suffix}]") { Source = EditorViewModel };
            AssociatedObject.SetBinding(undefinedProperty.DependencyProperty, undefinedMapBinding);
        }

        ObjectPropertyName = null;
    }
}
using System.Windows;
using System.Windows.Data;
using LogicSimulator.Controls;
using Microsoft.Xaml.Behaviors;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using SharpDX;

namespace LogicSimulator.Infrastructure.Behaviors;

public class PropertyEditorBehavior : Behavior<FrameworkElement>
{
    #region IsUndefinedValueDependencyProperty

    public DependencyProperty IsUndefinedValueDependencyProperties
    {
        get => (DependencyProperty) GetValue(IsUndefinedValueDependencyPropertiesProperty);
        set => SetValue(IsUndefinedValueDependencyPropertiesProperty, value);
    }

    public static readonly DependencyProperty IsUndefinedValueDependencyPropertiesProperty =
        DependencyProperty.Register(nameof(IsUndefinedValueDependencyProperties), typeof(DependencyProperty), typeof(PropertyEditorBehavior), new PropertyMetadata(null));

    #endregion

    #region EditorDependencyProperty

    public DependencyProperty EditorDependencyProperty
    {
        get => (DependencyProperty)GetValue(EditorDependencyPropertyProperty);
        set => SetValue(EditorDependencyPropertyProperty, value);
    }

    public static readonly DependencyProperty EditorDependencyPropertyProperty =
        DependencyProperty.Register(nameof(EditorDependencyProperty), typeof(DependencyProperty), typeof(PropertyEditorBehavior), new PropertyMetadata(default(DependencyProperty)));

    #endregion

    #region ObjectProperty

    public ObjectProperty ObjectProperty
    {
        get => (ObjectProperty)GetValue(ObjectPropertyProperty);
        set => SetValue(ObjectPropertyProperty, value);
    }

    public static readonly DependencyProperty ObjectPropertyProperty =
        DependencyProperty.Register(nameof(ObjectProperty), typeof(ObjectProperty), typeof(PropertyEditorBehavior), new PropertyMetadata(default(ObjectProperty)));

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
        
        var prop = behavior.ObjectProperty;
        var vm = e.NewValue as AbstractEditorViewModel;
        
        var propertyBinding = new Binding(prop.ObjectPropertyName) { Source = vm };
        behavior.AssociatedObject.SetBinding(behavior.EditorDependencyProperty, propertyBinding);

        if (prop.ObjectPropertyType == typeof(Vector2))
        {
            var undefinedMapBinding1 = new Binding($"UndefinedPropertiesMap[{prop.ObjectPropertyName + "X"}]") { Source = vm };
            behavior.AssociatedObject.SetBinding(Vector2Box.IsVectorXUndefinedProperty, undefinedMapBinding1);
            var undefinedMapBinding2 = new Binding($"UndefinedPropertiesMap[{prop.ObjectPropertyName + "Y"}]") { Source = vm };
            behavior.AssociatedObject.SetBinding(Vector2Box.IsVectorYUndefinedProperty, undefinedMapBinding2);
        }
        else
        {
            var undefinedMapBinding = new Binding($"UndefinedPropertiesMap[{prop.ObjectPropertyName}]") { Source = vm };
            behavior.AssociatedObject.SetBinding(behavior.IsUndefinedValueDependencyProperties, undefinedMapBinding);
        }
    }

    #endregion
}
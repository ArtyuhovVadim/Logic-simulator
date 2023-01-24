using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LogicSimulator.Controls;

public class ComboBoxEx : ComboBox
{
    #region IsValueUndefined

    public bool IsValueUndefined
    {
        get => (bool)GetValue(IsValueUndefinedProperty);
        set => SetValue(IsValueUndefinedProperty, value);
    }

    public static readonly DependencyProperty IsValueUndefinedProperty =
        DependencyProperty.Register(nameof(IsValueUndefined), typeof(bool), typeof(ComboBoxEx), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    #endregion

    static ComboBoxEx()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxEx), new FrameworkPropertyMetadata(typeof(ComboBoxEx)));
    }

    public ComboBoxEx()
    {
        DependencyPropertyDescriptor.FromProperty(SelectedItemProperty, typeof(ComboBoxEx)).AddValueChanged(this, OnSelectedItemChanged);

        PreviewMouseWheel += OnMouseWheel;
    }

    private void OnSelectedItemChanged(object sender, EventArgs args)
    {
        if (IsDropDownOpen)
        {
            IsValueUndefined = false;
        }
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs args)
    {
        if (IsFocused)
        {
            IsValueUndefined = false;
        }
    }
}
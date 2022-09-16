using System.Windows;

namespace LogicSimulator.Infrastructure.DataPiping;

public class DataPipe : Freezable
{
    #region Source

    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(object), typeof(DataPipe), new FrameworkPropertyMetadata(null, OnSourceChanged));

    private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((DataPipe)d).Target = e.NewValue;

    #endregion

    #region Target

    public object Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public static readonly DependencyProperty TargetProperty =
        DependencyProperty.Register(nameof(Target), typeof(object), typeof(DataPipe), new FrameworkPropertyMetadata(null));

    #endregion

    protected override Freezable CreateInstanceCore() => new DataPipe();
}
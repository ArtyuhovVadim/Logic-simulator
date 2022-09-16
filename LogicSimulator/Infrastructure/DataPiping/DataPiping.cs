using System.Windows;

namespace LogicSimulator.Infrastructure.DataPiping;

public static class DataPiping
{
    public static readonly DependencyProperty DataPipesProperty =
        DependencyProperty.RegisterAttached("DataPipes", typeof(DataPipeCollection), typeof(DataPiping), new UIPropertyMetadata(null));

    public static void SetDataPipes(DependencyObject o, DataPipeCollection value) => o.SetValue(DataPipesProperty, value);

    public static DataPipeCollection GetDataPipes(DependencyObject o) => (DataPipeCollection)o.GetValue(DataPipesProperty);
}
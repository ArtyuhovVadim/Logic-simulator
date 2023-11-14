using System.Windows;

namespace LogicSimulator.Infrastructure.DataPiping;

public static class DataPiping
{
    private static readonly DependencyProperty DataPipesProperty =
        DependencyProperty.RegisterAttached("ShadowDataPipes", typeof(DataPipeCollection), typeof(DataPiping), new FrameworkPropertyMetadata(default(DataPipeCollection)));

    public static DataPipeCollection GetDataPipes(DependencyObject o)
    {
        var collection = (DataPipeCollection)o.GetValue(DataPipesProperty);

        if (collection is not null) return collection;

        collection = new DataPipeCollection();
        o.SetValue(DataPipesProperty, collection);

        return collection;
    }
}
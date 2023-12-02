using System.Windows;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class MultiObjectsEditorViewModel(IEnumerable<EditorLayout> layouts) : EditorViewModel
{
    protected override EditorLayout CreateLayout() => GenerateLayout(GenerateProps());

    private EditorLayout GenerateLayout(IEnumerable<(string name, List<GridLength> layout, IEnumerable<PropertyViewModel> props)> propsPairs) => LayoutBuilder
        .Create(this)
        .WithName("Разные объекты")
        .WithGroup(builder =>
        {
            builder.WithGroupName("Общие свойства");

            foreach (var (name, layout, props) in propsPairs)
            {
                builder.WithRow(rowBuilder =>
                {
                    rowBuilder.WithRowName(name).WithLayout(layout);

                    foreach (var prop in props)
                    {
                        rowBuilder.WithProperty(prop);
                    }
                });
            }
        })
        .Build();

    private IEnumerable<(string name, List<GridLength> layout, IEnumerable<PropertyViewModel> props)> GenerateProps()
    {
        var comparer = new PropertyViewModelComparer();

        var propTuples = layouts
            .SelectMany(x => x.Groups)
            .SelectMany(group => group.EditorRows)
            .SelectMany(row => row.ObjectProperties.Select(x => (name: row.Name, prop: x, layout: row.Layout)));

        var props = layouts
            .Select(layout => layout.Groups.SelectMany(group => group.EditorRows).SelectMany(row => row.ObjectProperties))
            .Aggregate((current, property) => current.Intersect(property, comparer));

        var result = props
            .Select(prop => (
                name: string.Join('/', propTuples.Where(pair => comparer.Equals(pair.prop, prop)).Select(x => x.name).Distinct().Where(x => x != string.Empty)),
                layout: propTuples.First(pair => comparer.Equals(pair.prop, prop)).layout,
                prop: prop.MakeCopy(this)))
            .GroupBy(x => x.name)
            .Select(x => (name: x.Key, layout: x.First().layout, props: x.Select(pair => pair.prop)));

        return result;
    }
}

file class PropertyViewModelComparer : IEqualityComparer<PropertyViewModel>
{
    public bool Equals(PropertyViewModel x, PropertyViewModel y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;

        return x switch
        {
            SinglePropertyViewModel a when y is SinglePropertyViewModel b => a.PropertyName == b.PropertyName,
            MultiPropertyViewModel a when y is MultiPropertyViewModel b => a.Properties.Values.All(a1 =>
                b.Properties.Values.Count(b2 => Equals(a1, b2)) == 1),
            _ => false
        };
    }

    public int GetHashCode(PropertyViewModel obj)
    {
        return obj switch
        {
            SinglePropertyViewModel o => HashCode.Combine(o.PropertyName),
            MultiPropertyViewModel o => o.Properties.Values.Select(x => x.PropertyName.GetHashCode()).Aggregate(HashCode.Combine),
            _ => throw new InvalidOperationException()
        };
    }
}